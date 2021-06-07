// MSIX Hero
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

using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.Enums;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.ViewModels
{
    public class OperatingSystemDependencyViewModel : NotifyPropertyChanged
    {
        private readonly AppxOperatingSystemDependency _model;

        public OperatingSystemDependencyViewModel(AppxOperatingSystemDependency model)
        {
            this._model = model;
        }

        public AppxTargetOperatingSystemType IsMsixNativeSupported
        {
            get => this._model.Minimum?.IsNativeMsixPlatform ?? this._model.Tested?.IsNativeMsixPlatform ?? AppxTargetOperatingSystemType.Other;
        }

        public string FamilyName
        {
            get => this._model.Minimum?.NativeFamilyName ?? this._model.Tested?.NativeFamilyName;
        }

        public string MinimumDisplayName
        {
            get
            {
                if (this._model.Minimum == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(this._model.Minimum.MarketingCodename))
                {
                    return this._model.Minimum.Name;
                }

                return $"{this._model.Minimum.Name} ({this._model.Minimum.MarketingCodename})";
            }
        }

        public string MinimumVersion
        {
            get
            {
                if (this._model.Minimum == null)
                {
                    return null;
                }

                return this._model.Minimum.TechnicalVersion;
            }
        }

        public string TestedDisplayName
        {
            get
            {
                if (this._model.Tested == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(this._model.Tested.MarketingCodename))
                {
                    return this._model.Tested.Name;
                }

                return $"{this._model.Tested.Name} ({this._model.Tested.MarketingCodename})";
            }
        }

        public string TestedVersion
        {
            get
            {
                if (this._model.Tested == null)
                {
                    return null;
                }

                return this._model.Tested.TechnicalVersion;
            }
        }

        public bool HasMinimumVersion => this._model.Minimum != null;

        public bool HasTestedVersion => this._model.Tested != null;
    }
}
