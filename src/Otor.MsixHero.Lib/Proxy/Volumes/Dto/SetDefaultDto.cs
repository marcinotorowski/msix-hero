﻿using Otor.MsixHero.Appx.Volumes.Entities;

namespace Otor.MsixHero.Lib.Proxy.Volumes.Dto
{
    public class SetDefaultDto : ProxyObject
    {
        public SetDefaultDto()
        {
        }

        public SetDefaultDto(string drivePath)
        {
            this.DrivePath = drivePath;
        }

        public SetDefaultDto(AppxVolume volume) : this(volume.PackageStorePath)
        {
        }

        public string DrivePath { get; set; }
    }
}