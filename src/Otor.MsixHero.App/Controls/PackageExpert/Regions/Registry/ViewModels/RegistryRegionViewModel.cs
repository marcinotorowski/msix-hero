using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Registry.ViewModels
{
    public class RegistryRegionViewModel : PackageExpertPackageRegionViewModel
    {
        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.RegistryTree = new AppxRegistryViewModel(sourcePackage.PackageFile);
            this.OnPropertyChanged(nameof(this.RegistryTree));
            return Task.CompletedTask;
        }

        public AppxRegistryViewModel RegistryTree { get; private set; }
    }
}
