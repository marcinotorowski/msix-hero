using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Events
{
    public class AppxManifestPayloadCount
    {
        public AppxManifestPayloadCount(AppxPackage package, int payloadCount)
        {
            Package = package;
            PayloadCount = payloadCount;
        }

        public AppxPackage Package { get; }

        public int PayloadCount { get; }
    }
}