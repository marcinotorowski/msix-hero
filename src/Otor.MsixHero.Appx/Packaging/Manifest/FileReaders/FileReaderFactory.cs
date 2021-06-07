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
using Otor.MsixHero.Appx.Packaging.Installation.Enums;

namespace Otor.MsixHero.Appx.Packaging.Manifest.FileReaders
{
    public class FileReaderFactory
    {
        public static IAppxFileReader CreateFileReader(string pathOrPackageName)
        {
            if (string.IsNullOrEmpty(pathOrPackageName))
            {
                throw new ArgumentNullException(nameof(pathOrPackageName));
            }

            if (!File.Exists(pathOrPackageName))
            {
                if (!Directory.Exists(pathOrPackageName))
                {
                    throw new FileNotFoundException("File not found.", pathOrPackageName);
                }

                return new DirectoryInfoFileReaderAdapter(pathOrPackageName);
            }

            var reg = Regex.Match(pathOrPackageName, @"^([\w\.\-]+)(?:_~)?_[a-z0-9]{13}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (reg.Success)
            {
                // package full name
                return new PackageIdentityFileReaderAdapter(PackageContext.CurrentUser, pathOrPackageName);
            }

            // source is a file
            var fileName = Path.GetFileName(pathOrPackageName);
            if (string.Equals(FileConstants.AppxManifestFile, fileName, StringComparison.OrdinalIgnoreCase))
            {
                return new FileInfoFileReaderAdapter(pathOrPackageName);
            }

            var ext = Path.GetExtension(pathOrPackageName);
            switch (ext.ToLowerInvariant())
            {
                case FileConstants.MsixExtension:
                case FileConstants.AppxExtension:
                    return new ZipArchiveFileReaderAdapter(pathOrPackageName);
                default:
                    return new FileInfoFileReaderAdapter(pathOrPackageName);
            }
        }
    }
}
