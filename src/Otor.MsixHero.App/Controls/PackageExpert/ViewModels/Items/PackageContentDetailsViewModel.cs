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
using Otor.MsixHero.Appx.Packaging.Manifest.Entities.Sources;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items
{
    public class PackageContentDetailsViewModel : NotifyPropertyChanged
    {
        public PackageContentDetailsViewModel(AppxPackage model, string filePath = null)
        {
            this.Model = model;
            this.DisplayName = model.DisplayName;
            this.Description = model.Description;
            this.Publisher = model.Publisher;
            this.FamilyName = model.FamilyName;
            this.Architecture = model.ProcessorArchitecture.ToString();
            this.PackageFullName = model.FullName;
            this.PublisherDisplayName = model.PublisherDisplayName;
            this.Version = model.Version;
            this.Logo = model.Logo;
            
            this.ScriptsCount = 0;

            if (model.Applications != null)
            {
                foreach (var item in model.Applications)
                {
                    if (string.IsNullOrEmpty(this.TileColor))
                    {
                        this.TileColor = item.BackgroundColor;
                    }
                    
                    this.ScriptsCount += item.Psf?.Scripts?.Count ?? 0;
                    this.ApplicationCount++;
                }
            }
            
            if (string.IsNullOrEmpty(this.TileColor))
            {
                this.TileColor = "#666666";
            }
            
            this.PackageIntegrity = model.PackageIntegrity;
            this.RootDirectory = filePath;
        }

        public string RootDirectory { get; }

        public AppxPackage Model { get; private set; }

        public bool HasAppInstallerUri => this.Model.Source is AppInstallerPackageSource;

        public bool PackageIntegrity { get; }
        
        public string PackageFullName { get; }

        public string Description { get; }

        public string PublisherDisplayName { get; }

        public string Publisher { get; }

        public string TileColor { get; }

        public string DisplayName { get; }

        public byte[] Logo { get; }

        public string FamilyName { get; }

        public string Architecture { get; }

        public string Version { get; }
        
        public int ScriptsCount { get; }

        public int ApplicationCount { get; }
    }
}
