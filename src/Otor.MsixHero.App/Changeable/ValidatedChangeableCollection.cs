﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Otor.MsixHero.App.Changeable
{
    public class ValidatedChangeableCollection<T> : ChangeableCollection<T>, IValidatedContainerChangeable, IDataErrorInfo
    {
        private string validationMessage;
        private bool isValidated = true;
        private IReadOnlyCollection<Func<IEnumerable<T>, string>> validators;
        private bool displayValidationErrors = true;

        public ValidatedChangeableCollection(Func<IEnumerable<T>, string> validator = null)
        {
            if (validator != null)
            {
                this.validators = new List<Func<IEnumerable<T>, string>> { validator };
            }
        }

        public ValidatedChangeableCollection(Func<IEnumerable<T>, string> validator, IEnumerable<T> items) : base(items)
        {
            if (validator != null)
            {
                this.validators = new List<Func<IEnumerable<T>, string>> { validator };
            }
        }

        public string Error => this.ValidationMessage;

        public string this[string columnName] => null;

        protected override void ClearItems()
        {
            base.ClearItems();
            this.Validate();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (item is IValidatedChangeable validatedItem)
            {
                validatedItem.ValidationStatusChanged -= this.ItemOnValidationStatusChanged;
                validatedItem.ValidationStatusChanged += this.ItemOnValidationStatusChanged;
                validatedItem.DisplayValidationErrors = this.DisplayValidationErrors;
            }

            this.Validate();
        }

        private void ItemOnValidationStatusChanged(object sender, ValueChangedEventArgs<string> e)
        {
            if (string.IsNullOrEmpty(e.NewValue))
            {
                this.Validate();
            }
            else
            {
                this.Validate(e.NewValue);
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
            this.Validate();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (item is IValidatedChangeable validatedItem)
            {
                validatedItem.ValidationStatusChanged -= this.ItemOnValidationStatusChanged;
            }

            base.RemoveItem(index);
            this.Validate();
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];
            if (oldItem is IValidatedChangeable oldValidatedItem)
            {
                oldValidatedItem.ValidationStatusChanged -= this.ItemOnValidationStatusChanged;
            }

            base.SetItem(index, item);

            if (item is IValidatedChangeable newValidatedItem)
            {
                newValidatedItem.ValidationStatusChanged += this.ItemOnValidationStatusChanged;
                newValidatedItem.DisplayValidationErrors = this.DisplayValidationErrors;
            }
            
            this.Validate();
        }

        public string ValidationMessage
        {
            get => this.validationMessage;
            private set
            {
                var oldIsValid = string.IsNullOrEmpty(this.validationMessage);
                if (!this.SetField(ref this.validationMessage, value))
                {
                    return;
                }

                var newIsValid = string.IsNullOrEmpty(value);
                if (oldIsValid != newIsValid)
                {
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.IsValid)));
                }

                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Error)));
            }
        }

        public bool IsValidated
        {
            get => this.isValidated;
            set
            {
                if (!this.SetField(ref this.isValidated, value))
                {
                    return;
                }

                this.Validate();
            }
        }

        public bool IsValid => string.IsNullOrEmpty(this.validationMessage);

        public bool DisplayValidationErrors
        {
            get => this.displayValidationErrors;
            set
            {
                this.SetField(ref this.displayValidationErrors, value);

                foreach (var item in this.OfType<IValidatedChangeable>())
                {
                    item.DisplayValidationErrors = value;
                }
            }
        }

        public IReadOnlyCollection<Func<IEnumerable<T>, string>> Validators
        {
            get => this.validators;
            set
            {
                this.validators = value;
                this.Validate();
            }
        }

        public event EventHandler<ValueChangedEventArgs<string>> ValidationStatusChanged;
        
        private void Validate(string errorMessage = null)
        {
            var oldValidationMessage = this.ValidationMessage;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ValidationMessage = errorMessage;
            }
            else
            {
                if (!this.IsValidated || this.Validators == null || !this.Validators.Any())
                {
                    this.ValidationMessage = null;
                }
                else
                {
                    string msg = null;
                    foreach (var validator in this.Validators)
                    {
                        msg = validator(this);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            break;
                        }
                    }

                    this.ValidationMessage = msg;
                }
            }

            // ReSharper disable once InvertIf
            if (oldValidationMessage != this.ValidationMessage)
            {
                var validationChanged = this.ValidationStatusChanged;
                if (validationChanged != null)
                {
                    validationChanged(this, new ValueChangedEventArgs<string>(this.ValidationMessage));
                }
            }
        }
    }
}