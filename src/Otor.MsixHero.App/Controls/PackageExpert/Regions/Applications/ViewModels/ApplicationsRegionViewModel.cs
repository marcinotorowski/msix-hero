using System.Collections.Generic;
using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Applications.ViewModels
{
    public class ApplicationsRegionViewModel : PackageExpertPackageRegionViewModel
    {
        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.Applications = new List<AppxApplicationViewModel>();

            foreach (var item in sourcePackage.Applications)
            {
                this.Applications.Add(new AppxApplicationViewModel(item, sourcePackage));
            }

            this.OnPropertyChanged(nameof(this.Applications));
            return Task.CompletedTask;
        }

        public IList<AppxApplicationViewModel> Applications { get; private set; }
    }
}
