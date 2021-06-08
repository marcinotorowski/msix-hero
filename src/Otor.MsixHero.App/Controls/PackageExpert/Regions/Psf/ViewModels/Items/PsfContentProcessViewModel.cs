﻿// MSIX Hero
// Copyright (C) 2021 Marcin Otorowski
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// Full notice:
// https://github.com/marcinotorowski/msix-hero/blob/develop/LICENSE.md

using System;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.Model;
using Otor.MsixHero.App.Mvvm.Changeable;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels.Items
{
    public abstract class PsfContentProcessViewModel : PsfContentRegexpViewModel
    {
        private readonly ChangeableProperty<Bitness> is64Bit;

        protected PsfContentProcessViewModel(string processRegularExpression, string fixupName) : base(processRegularExpression)
        {
            Bitness bitness;
            if (fixupName.IndexOf("64", StringComparison.Ordinal) != -1)
            {
                bitness = Bitness.x64;
            }
            else if (fixupName.IndexOf("32", StringComparison.Ordinal) != -1 || fixupName.IndexOf("86", StringComparison.Ordinal) != -1)
            {
                bitness = Bitness.x86;
            }
            else
            {
                bitness = Bitness.Unknown;
            }

            this.is64Bit = new ChangeableProperty<Bitness>(bitness);
            this.AddChildren(this.is64Bit);
        }

        public Bitness Is64Bit
        {
            get => this.is64Bit.CurrentValue;
            set
            {
                if (this.is64Bit.CurrentValue == value)
                {
                    return;
                }

                this.is64Bit.CurrentValue = value;
                this.OnPropertyChanged();
            }
        }

        protected sealed override void SetValuesFromRegularExpression(string expr)
        {
            var interpretation = new RegexpInterpreter(expr, false);

            switch (interpretation.Result)
            {
                case InterpretationResult.Any:
                    this.TextBefore = null;
                    this.DisplayText = "any ";
                    this.TextAfter = "process";
                    break;
                case InterpretationResult.Name:
                    this.TextBefore = "process ";
                    this.DisplayText = interpretation.DisplayText;
                    this.TextAfter = null;
                    break;
                case InterpretationResult.StartsWith:
                    this.TextBefore = "processes that start with ";
                    this.DisplayText = interpretation.DisplayText;
                    this.TextAfter = null;
                    break;
                case InterpretationResult.EndsWith:
                    this.TextBefore = "processes that end with ";
                    this.DisplayText = interpretation.DisplayText;
                    this.TextAfter = null;
                    break;
                default:
                    this.TextBefore = "processes that match pattern ";
                    this.DisplayText = interpretation.RegularExpression;
                    this.TextAfter = null;
                    break;
            }
        }
    }
}