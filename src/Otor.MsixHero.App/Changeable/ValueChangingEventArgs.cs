﻿namespace Otor.MsixHero.App.Changeable
{
    public class ValueChangingEventArgs : ValueChangedEventArgsBase
    {
        public ValueChangingEventArgs(object oldValue, object newValue, object originalValue, bool wasDirty, bool wasTouched) : base(oldValue, newValue, originalValue, wasDirty, wasTouched)
        {
        }

        public bool Cancel { get; set; }
    }
}