using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Capabilities.ViewModels
{
    public class CapabilitiesRegionViewModel : PackageExpertPackageRegionViewModel
    {
        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.SetCapabilities(sourcePackage.Capabilities);
            return Task.CompletedTask;
        }
        
        private void SetCapabilities(IEnumerable<AppxCapability> appxPackageCapabilities)
        {
            this.Count = 0;
            foreach (var c in appxPackageCapabilities.GroupBy(c => c.Type))
            {
                switch (c.Key)
                {
                    case CapabilityType.General:
                        this.General = new List<CapabilityViewModel>(c.Select(cap => new CapabilityViewModel(cap)));
                        this.Count += this.General.Count;
                        break;
                    case CapabilityType.Restricted:
                        this.Restricted = new List<CapabilityViewModel>(c.Select(cap => new CapabilityViewModel(cap)));
                        this.Count += this.Restricted.Count;
                        break;
                    case CapabilityType.Device:
                        this.Device = new List<CapabilityViewModel>(c.Select(cap => new CapabilityViewModel(cap)));
                        this.Count += this.Device.Count;
                        break;
                    default:
                        this.Custom = new List<CapabilityViewModel>(c.Select(cap => new CapabilityViewModel(cap)));
                        this.Count += this.Custom.Count;
                        break;
                }
            }

            this.OnPropertyChanged(null);
        }


        public int Count { get; private set; }

        public IReadOnlyCollection<CapabilityViewModel> General { get; private set; }

        public IReadOnlyCollection<CapabilityViewModel> Restricted { get; private set; }

        public IReadOnlyCollection<CapabilityViewModel> Device { get; private set; }

        public IReadOnlyCollection<CapabilityViewModel> Custom { get; private set; }

        public bool HasGeneral => this.General?.Any() == true;

        public bool HasDevice => this.Device?.Any() == true;

        public bool HasCustom => this.Custom?.Any() == true;

        public bool HasRestricted => this.Restricted?.Any() == true;
    }
}
