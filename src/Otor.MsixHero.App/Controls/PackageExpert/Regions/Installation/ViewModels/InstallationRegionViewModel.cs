using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Installation.ViewModels
{
    public class InstallationRegionViewModel : PackageExpertPackageRegionViewModel
    {
        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.Source = sourcePackage.Source;
            this.OnPropertyChanged(nameof(this.Source));
            return Task.CompletedTask;
        }

        public AppxSource Source { get; private set; }
    }
}
