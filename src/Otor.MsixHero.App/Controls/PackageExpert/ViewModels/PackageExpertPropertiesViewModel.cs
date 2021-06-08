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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.ViewModels;
using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Appx.Packaging.Installation.Enums;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities.Sources;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels
{
    public class PackageExpertPropertiesViewModel : NotifyPropertyChanged
    {
        public PackageExpertPropertiesViewModel(AppxPackage model)
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

            this.OperatingSystemDependencies = new ObservableCollection<OperatingSystemDependencyViewModel>();
            this.PackageDependencies = new ObservableCollection<PackageDependencyViewModel>();

            if (model.OperatingSystemDependencies != null)
            {
                foreach (var item in model.OperatingSystemDependencies)
                {
                    this.OperatingSystemDependencies.Add(new OperatingSystemDependencyViewModel(item));
                }
            }

            if (model.PackageDependencies != null)
            {
                foreach (var item in model.PackageDependencies)
                {
                    this.PackageDependencies.Add(new PackageDependencyViewModel(item));
                }
            }

            this.HasOperatingSystemDependencies = this.OperatingSystemDependencies.Any();
            this.HasPackageDependencies = this.PackageDependencies.Any();

            this.ScriptsCount = 0;

            if (model.Applications != null)
            {
                foreach (var item in model.Applications)
                {
                    if (string.IsNullOrEmpty(this.TileColor))
                    {
                        this.TileColor = item.BackgroundColor;
                    }

                    this.ApplicationsCount++;
                    this.ScriptsCount += item.Psf?.Scripts?.Count ?? 0;
                }
            }
            
            if (string.IsNullOrEmpty(this.TileColor))
            {
                this.TileColor = "#666666";
            }
            
            if (model.PackageFile != null)
            {
                this.RootDirectory = model.PackageFile.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\'), "%programfiles%");
                this.PsfFilePath = Path.Combine(model.PackageFile, "config.json");
                this.ManifestFilePath = Path.Combine(model.PackageFile, FileConstants.AppxManifestFile);
            }

            this.CapabilitiesCount = model.Capabilities.Count;
            this.PackageIntegrity = model.PackageIntegrity;
            
            this.UserDirectory = Path.Combine("%localappdata%", "Packages", this.FamilyName, "LocalCache");

            if (!File.Exists(this.PsfFilePath))
            {
                this.PsfFilePath = null;
            }

            if (!File.Exists(this.ManifestFilePath))
            {
                this.ManifestFilePath = null;
            }
        }
        
        public string RootDirectory { get; }

        public string UserDirectory { get; }

        public int CapabilitiesCount { get; }

        public string PsfFilePath { get; }

        public string ManifestFilePath { get; }

        public AppxPackage Model { get; private set; }

        public string AppType
        {
            get
            {
                if (this.Model.Source is StorePackageSource)
                {
                    return "Store App";
                }

                if (this.Model.Source is SystemSource)
                {
                    return "System App";
                }

                if (this.Model.Source is DeveloperSource)
                {
                    return "Developer";
                }

                if (this.Model.Source is NotInstalledSource)
                {
                    return "Not installed";
                }

                return "Sideloaded App";
            }
        }

        public string AppTypeTooltip
        {
            get
            {
                if (this.Model.Source is StorePackageSource)
                {
                    return "This application has been installed from Microsoft Store.";
                }

                if (this.Model.Source is SystemSource)
                {
                    return "This application has been pre-installed with Windows.";
                }

                if (this.Model.Source is DeveloperSource)
                {
                    return "This application has been installed from a manifest file, using the Developer mode.";
                }

                if (this.Model.Source is NotInstalledSource)
                {
                    return "This application has not been installed yet.";
                }

                if (this.Model.Source is AppInstallerPackageSource)
                {
                    return "This application has been side-loaded from an .appinstaller file.";
                }

                return "This application has been side-loaded.";
            }
        }

        public string Caption
        {
            get
            {
                var result = this.Model.IsFramework ? MsixPackageType.Framework : 0;
                foreach (var app in this.Model.Applications ?? Enumerable.Empty<AppxApplication>())
                {
                    result = PackageTypeConverter.GetPackageTypeFrom(app.EntryPoint, app.Executable, app.StartPage, this.Model.IsFramework);
                    break;
                }

                switch (result)
                {
                    case MsixPackageType.Uwp:
                        return "UWP";
                    case MsixPackageType.BridgeDirect:
                        return "Win32";
                    case MsixPackageType.BridgePsf:
                        return "Win32 + PSF";
                    case MsixPackageType.Web:
                        return "Web";
                    case MsixPackageType.Framework:
                        return "Framework";
                    default:
                        return null;
                }
            }
        }
        public string CaptionToolTip
        {
            get
            {
                var result = this.Model.IsFramework ? MsixPackageType.Framework : 0;
                foreach (var app in this.Model.Applications ?? Enumerable.Empty<AppxApplication>())
                {
                    result = PackageTypeConverter.GetPackageTypeFrom(app.EntryPoint, app.Executable, app.StartPage, this.Model.IsFramework);
                    break;
                }

                switch (result)
                {
                    case MsixPackageType.Uwp:
                        return "This is Universal Windows Platform (UWP) package.";
                    case MsixPackageType.BridgeDirect:
                        return "This is a Win32 packaged app.";
                    case MsixPackageType.BridgePsf:
                        return "This is a Win32 packaged app, enhanced by Package Support Framework (PSF)";
                    case MsixPackageType.Web:
                        return "This is a web app.";
                    case MsixPackageType.Framework:
                        return "This is a framework app";
                    default:
                        return "This is an app of an unknown type.";
                }
            }
        }

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

        public ObservableCollection<OperatingSystemDependencyViewModel> OperatingSystemDependencies { get; }

        public bool HasOperatingSystemDependencies { get; }

        public bool HasPackageDependencies { get; }

        public ObservableCollection<PackageDependencyViewModel> PackageDependencies { get; }

        public int ApplicationsCount { get; }
        
        public int ScriptsCount { get; }
    }
}
