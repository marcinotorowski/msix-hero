using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.ViewModels
{
    public class DependenciesRegionViewModel : PackageExpertPackageRegionViewModel
    {
        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.OperatingSystemDependencies = new List<OperatingSystemDependencyViewModel>();
            if (sourcePackage.OperatingSystemDependencies != null)
            {
                foreach (var item in sourcePackage.OperatingSystemDependencies)
                {
                    this.OperatingSystemDependencies.Add(new OperatingSystemDependencyViewModel(item));
                }
            }

            this.PackageDependencies = new List<PackageDependencyViewModel>();
            if (sourcePackage.PackageDependencies != null)
            {
                foreach (var item in sourcePackage.PackageDependencies)
                {
                    this.PackageDependencies.Add(new PackageDependencyViewModel(item));
                }
            }

            this.HasOperatingSystemDependencies = this.OperatingSystemDependencies.Any();
            this.HasPackageDependencies = this.PackageDependencies.Any();

            this.OnPropertyChanged(null);
            return Task.CompletedTask;
        }

        public bool HasPackageDependencies { get; private set; }

        public IList<OperatingSystemDependencyViewModel> OperatingSystemDependencies { get; private set; }

        public bool HasOperatingSystemDependencies { get; private set; }

        public IList<PackageDependencyViewModel> PackageDependencies { get; private set; }
    }
}
