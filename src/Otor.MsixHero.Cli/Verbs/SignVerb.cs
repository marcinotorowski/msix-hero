﻿using System.Collections.Generic;
using CommandLine;
using Org.BouncyCastle.Ocsp;
using Otor.MsixHero.Appx.Signing;

namespace Otor.MsixHero.Cli.Verbs
{
    [Verb("sign", HelpText = "Sign a package")]
    public class SignVerb
    {
        [Value(1, MetaName = "file path", HelpText = "Full paths to one or more files (separated by space).")]
        public IEnumerable<string> FilePath { get; set; }

        [Option("sm", HelpText = "Specifies that a machine store, instead of a user store, is used.", Default = false, Required = false, SetName = "Installed certificate")]
        public bool UseMachineStore { get; set; }

        [Option('t', "timestamp", HelpText = "Specifies the URL of the RFC 3161 time stamp server. If not specified, the default value from MSIX Hero settings will be used.", Required = false)]
        public string TimeStampUrl { get; set; }
        
        [Option("sha1", HelpText = "Specifies the SHA1 hash of the signing certificate. The SHA1 hash is commonly specified when multiple certificates satisfy the criteria specified by the remaining switches.", Required = false, SetName = "Installed certificate")]
        public string ThumbPrint { get; set; }

        [Option('f', "file", HelpText = "Specifies the signing certificate in a file. If the file is in Personal Information Exchange (PFX) format and protected by a password, use the -p option to specify the password.", Required = false, SetName = "PFX")]
        public string PfxFilePath { get; set; }
        
        [Option('p', "password", HelpText = "	Specifies the password to use when opening a PFX file.", Required = false, SetName = "PFX")]
        public string PfxPassword { get; set; }

        [Option("dgf", HelpText = "Full path to JSON file containing access tokens to AzureAD.", SetName = "Device Guard Signing")]
        public string DeviceGuardFile { get; set; }

        [Option("dg", HelpText = "A switch for interactive Device Guard signing (you will be asked for AzureAD credentials).", SetName = "Device Guard Signing")]
        public bool DeviceGuardInteractive { get; set; }

        [Option("dgv1", Default = false, Required = false, HelpText = "A switch determining whether the obsolete Device Guard Signing Service version 1 will be used. Using this parameter is not recommended.", SetName = "Device Guard Signing")]
        public bool DeviceGuardVersion1 { get; set; }

        [Option("dgp", HelpText = "Publisher name used for signing with Device Guard.", SetName = "Device Guard Signing")]
        public string DeviceGuardSubject { get; set; }

        [Option('i', "increaseVersion", HelpText = "Specifies whether the version should be automatically increased, and (if yes) which component of it. Supported values are [None, Major, Minor, Build, Revision].", Default = IncreaseVersionMethod.None, Required = false)]
        public IncreaseVersionMethod IncreaseVersion { get; set; }
    }
}
