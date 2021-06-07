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

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.ViewModels
{
    public class PackageDependencyViewModel : NotifyPropertyChanged
    {
        private readonly AppxPackageDependency _model;

        public PackageDependencyViewModel(AppxPackageDependency model)
        {
            this._model = model;
        }

        public string Version => this._model.Dependency?.Version ?? this._model.Version;

        public string DisplayName => this._model.Dependency?.DisplayName ?? this._model.Name;

        public string DisplayPublisherName => this._model.Dependency?.PublisherDisplayName ?? this._model.Publisher;

        public string Name => this._model.Name;

        public string Publisher => this._model.Publisher;
    }
}