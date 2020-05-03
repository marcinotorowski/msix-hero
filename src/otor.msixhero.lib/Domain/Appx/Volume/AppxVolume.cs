﻿using System;
using System.Xml.Serialization;

namespace otor.msixhero.lib.Domain.Appx.Volume
{
    [Serializable]
    public class AppxVolume
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string PackageStorePath { get; set; }

        [XmlAttribute]
        public bool IsDefault { get; set; }

        [XmlAttribute]
        public long Capacity { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }

        [XmlAttribute]
        public long AvailableFreeSpace { get; set; }

        [XmlAttribute]
        public bool IsDriveReady { get; set; }

        [XmlAttribute]
        public bool IsOffline { get; set; }

        [XmlAttribute]
        public bool IsSystem { get; set; }
    }
}
