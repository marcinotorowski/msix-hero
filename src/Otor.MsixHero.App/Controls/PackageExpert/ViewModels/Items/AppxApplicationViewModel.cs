﻿using System;
using System.Linq;
using Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Psf;
using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Appx.Packaging.Installation.Enums;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items
{
    public class AppxApplicationViewModel : NotifyPropertyChanged
    {
        private readonly AppxApplication model;
        private readonly AppxPackage package;

        public AppxApplicationViewModel(AppxApplication model, AppxPackage package)
        {
            this.model = model;
            this.package = package;
            this.Psf = model.Psf == null ? null : new AppxPsfViewModel(package.RootFolder, model.Psf);
            this.Services = model.Extensions == null ? null : new AppxServicesViewModel(model.Extensions);

            this.Type = PackageTypeConverter.GetPackageTypeFrom(this.model.EntryPoint, this.model.Executable, this.model.StartPage, this.package.IsFramework);
            this.Alias = this.model.ExecutionAlias?.Any() == true ? string.Join(", ", this.model.ExecutionAlias.Distinct(StringComparer.OrdinalIgnoreCase)) : null;
        }
        
        public bool Visible => this.model.Visible;

        public string Alias { get; }

        public bool HasPsf => this.model.Psf != null;
        
        public AppxPsfViewModel Psf { get; }

        public AppxServicesViewModel Services { get; }

        public string DisplayName => this.model.DisplayName;

        public byte[] Image => this.model.Logo;
        
        public string Id => this.model.Id;

        public string TileColor => this.model.BackgroundColor;

        public string Start
        {
            get
            {
                switch (PackageTypeConverter.GetPackageTypeFrom(this.model.EntryPoint, this.model.Executable, this.model.StartPage, this.package.IsFramework))
                {
                    case MsixPackageType.BridgeDirect:
                    case MsixPackageType.BridgePsf:
                        return this.model.Executable;
                    case MsixPackageType.Web:
                        return this.model.StartPage;
                    default:
                        if (string.IsNullOrEmpty(this.model.EntryPoint))
                        {
                            return this.model.Executable;
                        }

                        return this.model.Executable;
                }
            }
        }

        public string EntryPoint => this.Type == MsixPackageType.Uwp ? this.model.EntryPoint : null;

        public bool HasEntryPoint => this.Type == MsixPackageType.Uwp;

        public MsixPackageType Type { get; }
    }
}