﻿using System;

namespace Otor.MsixHero.App.Changeable
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public ValueChangedEventArgs(T newValue)
        {
            this.NewValue = newValue;
        }

        public T NewValue { get; }
    }
}