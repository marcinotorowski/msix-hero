﻿using System;
using otor.msixhero.lib.Domain.Appx.Manifest.Full;

namespace otor.msixhero.lib.BusinessLayer.Helpers
{
    public class Windows10Parser
    {
        public static AppxTargetOperatingSystem GetOperatingSystemFromNameAndVersion(string name, string version)
        {
            switch (name)
            {
                case "Windows.Desktop":
                    // throw new NotSupportedException();
                    return GetWindowsDesktop(version);
                case "Windows.Universal":
                    return GetWindowsDesktop(version);
                default:
                    return null;
            }
        }
        public static AppxTargetOperatingSystem GetOperatingSystemFromNameAndVersion(string version)
        {
            return GetOperatingSystemFromNameAndVersion("Windows.Desktop", version);
        }

        private static AppxTargetOperatingSystem GetWindowsDesktop(string version)
        {
            var result = new AppxTargetOperatingSystem();
            result.TechnicalVersion = version;

            if (version != null && Version.TryParse(version, out var parsedVersion))
            {
                version = parsedVersion.ToString(3);
                switch (version)
                {
                    case "10.0.10240":
                        result.MarketingCodename = "";
                        result.Name = "Windows 10 1507";
                        break;
                    case "10.0.10586":
                        result.MarketingCodename = "November Update";
                        result.Name = "Windows 10 1511";
                        break;
                    case "10.0.14393":
                        result.MarketingCodename = "Anniversary Update";
                        result.Name = "Windows 10 1607";
                        break;
                    case "10.0.15063":
                        result.MarketingCodename = "Creators Update";
                        result.Name = "Windows 10 1703";
                        break;
                    case "10.0.16299":
                        result.MarketingCodename = "Fall Creators Update";
                        result.Name = "Windows 10 1709";
                        break;
                    case "10.0.17134":
                        result.MarketingCodename = "April 2018 Update";
                        result.Name = "Windows 10 1803";
                        break;
                    case "10.0.17763":
                        result.MarketingCodename = "October 2018 Update";
                        result.Name = "Windows 10 1809";
                        break;
                    case "10.0.18362":
                        result.MarketingCodename = "May 2019 Update";
                        result.Name = "Windows 10 1903";
                        break;
                    case "10.0.18363":
                        result.MarketingCodename = "November 2019 Update";
                        result.Name = "Windows 10 1909";
                        break;
                    default:
                        result.Name = "Windows 10";
                        // throw new NotSupportedException();
                        return result;
                }
            }

            return result;
        }
    }
}
