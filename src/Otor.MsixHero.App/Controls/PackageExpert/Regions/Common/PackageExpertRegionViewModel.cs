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

using System.Threading.Tasks;
using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Infrastructure.Logging;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Common
{
    public abstract class PackageExpertRegionViewModel : NotifyPropertyChanged
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(PackageExpertPackageRegionViewModel));

        private object _previousSource;
        
        protected internal async Task SetSourceObject(object sourceObject)
        {
            if (sourceObject == null)
            {
                return;
            }

            if (this._previousSource == sourceObject)
            {
                return;
            }

            this._previousSource = sourceObject;
            await this.OnSourceChanged(this._previousSource).ConfigureAwait(false);
        }
        
        protected abstract Task OnSourceChanged(object sourceObject);
    }
}