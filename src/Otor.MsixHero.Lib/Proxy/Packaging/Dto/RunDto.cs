﻿using System;
using System.Xml.Serialization;
using Otor.MsixHero.Appx.Packaging.Installation.Entities;

namespace Otor.MsixHero.Lib.Proxy.Packaging.Dto
{
    [Serializable]
    public class RunDto : ProxyObject
    {
        public RunDto()
        {
        }

        public RunDto(string packageFamilyName, string manifestPath, string applicationId = null)
        {
            this.PackageFamilyName = packageFamilyName;
            this.ManifestPath = manifestPath;
            this.ApplicationId = applicationId;
        }

        public RunDto(InstalledPackage package, string applicationId = null) : this(package.PackageFamilyName, package.ManifestLocation, applicationId)
        {
        }
        
        [XmlElement]
        public string PackageFamilyName { get; set; }

        [XmlElement]
        public string ManifestPath { get; set; }

        [XmlElement]
        public string ApplicationId { get; set; }
    }
}