﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Windows.Data.Xml.Dom;
using otor.msixhero.lib.BusinessLayer.State.Enums;

namespace otor.msixhero.lib.BusinessLayer.Actions
{

    [Serializable]
    public class ReloadPackages : BaseElevatedAction
    {
        public ReloadPackages()
        {
        }

        public ReloadPackages(PackageContext context)
        {
            this.Context = context;
        }

        [XmlElement]
        public PackageContext? Context { get; set; }

        public override bool RequiresElevation
        {
            get
            {
                return this.Context == PackageContext.AllUsers;
            }
        }
    }
}
