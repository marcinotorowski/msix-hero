using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.General;
using Otor.MsixHero.App.Helpers;
using Otor.MsixHero.Appx.Packaging.Registry;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Registry.ViewModels
{
    public class AppxRegistryViewModel : TreeViewModel<AppxRegistryKeyViewModel, AppxRegistryValueViewModel>
    {
        public AppxRegistryViewModel(string packageFile) : base(packageFile)
        {
            this.Containers = new AsyncProperty<IList<AppxRegistryKeyViewModel>>(this.GetRootContainers());
            
            var nodesCollection = new ObservableCollection<AppxRegistryValueViewModel>();
            this.Nodes = nodesCollection;
        }

        public override bool IsAvailable => this.GetAppxReader().FileExists("registry.dat");

        public override AsyncProperty<IList<AppxRegistryKeyViewModel>> Containers { get; }

        public override ObservableCollection<AppxRegistryValueViewModel> Nodes { get; }
        
        public async Task<IList<AppxRegistryKeyViewModel>> GetRootContainers()
        {
            if (!this.IsAvailable)
            {
                return null;
            }
            
            var roots = new List<AppxRegistryKeyViewModel>();

            using var appxReader = this.GetAppxReader();
            await using var registry = appxReader.GetFile("Registry.dat");
            using var reader = new AppxRegistryReader(registry);
            await foreach (var root in reader.EnumerateKeys(AppxRegistryRoots.Root).ConfigureAwait(false))
            {
                roots.Add(new AppxRegistryKeyViewModel(this, root));
            }

            return roots;
        }
    }
}
