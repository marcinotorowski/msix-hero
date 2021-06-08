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
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Otor.MsixHero.Appx.Psf.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.Converters
{
    public class TraceLevelConverter : MarkupExtension, IValueConverter
    {
        // ReSharper disable once InconsistentNaming
        private static TraceLevelConverter instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            switch ((TraceLevel) value)
            {
                case TraceLevel.UnexpectedFailures:
                    return "Unexpected failures";
                case TraceLevel.Always:
                    return "Unexpected failures";
                case TraceLevel.IgnoreSuccess:
                    return "Ignore success";
                case TraceLevel.AllFailures:
                    return "All failures";
                case TraceLevel.Ignore:
                    return "Ignore";
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var me = instance ?? (instance = new TraceLevelConverter());
            return me;
        }
    }
}
