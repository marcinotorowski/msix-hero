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

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Appx.Packaging.Installation.Enums;
using Otor.MsixHero.Appx.Packaging.Manifest;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Common
{
    public abstract class PackageExpertPackageRegionViewModel : PackageExpertRegionViewModel
    {
        protected string CurrentPackagePath;
        protected string CurrentPackageFullName;

        protected override async Task OnSourceChanged(object sourceObject)
        {
            if (sourceObject == null)
            {
                return;
            }

            this.HasError = false;
            this.ErrorMessage = null;

            try
            {
                if (sourceObject is AppxPackage appx)
                {
                    this.CurrentPackagePath = appx.PackageFile;
                    this.CurrentPackageFullName = appx.FullName;
                    await this.OnSourceChanged(appx).ConfigureAwait(false);
                }
                else if (sourceObject is FileInfo fileInfo)
                {
                    using var reader = FileReaderFactory.CreateFileReader(fileInfo.FullName);
                    var manifestReader = new AppxManifestReader();
                    var manifest = await manifestReader.Read(reader).ConfigureAwait(false);

                    this.CurrentPackagePath = fileInfo.FullName;
                    this.CurrentPackageFullName = manifest.FullName;

                    await this.OnSourceChanged(manifest).ConfigureAwait(false);
                }
                else if (sourceObject is string fileOrAppId)
                {
                    var reg = Regex.Match(fileOrAppId, @"\b([\w\.\-]+)(?:_~)?_[a-z0-9]{13}\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    if (reg.Success)
                    {
                        // this is a package ID
                        using var reader = new PackageIdentityFileReaderAdapter(PackageContext.CurrentUser, fileOrAppId);
                        var manifestReader = new AppxManifestReader();
                        var manifest = await manifestReader.Read(reader).ConfigureAwait(false);

                        this.CurrentPackagePath = null;
                        this.CurrentPackageFullName = manifest.FullName;

                        await this.OnSourceChanged(manifest).ConfigureAwait(false);
                    }
                    else
                    {

                        // this may be a file path, let's try
                        using var reader = FileReaderFactory.CreateFileReader(fileOrAppId);
                        var manifestReader = new AppxManifestReader();
                        var manifest = await manifestReader.Read(reader).ConfigureAwait(false);
                        
                        this.CurrentPackagePath = fileOrAppId;
                        this.CurrentPackageFullName = manifest.FullName;

                        await this.OnSourceChanged(manifest).ConfigureAwait(false);
                    }
                }
                else
                {
                    this.HasError = true;
                    this.ErrorMessage = "Not supported source.";

                    this.CurrentPackageFullName = null;
                    this.CurrentPackagePath = null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                this.HasError = true;
                this.ErrorMessage = e.Message;
            }
        }

        public bool HasError { get; private set; }

        public string ErrorMessage { get; private set; }

        protected abstract Task OnSourceChanged(AppxPackage sourcePackage);
    }
}