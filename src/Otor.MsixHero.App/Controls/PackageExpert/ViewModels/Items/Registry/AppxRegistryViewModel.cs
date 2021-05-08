using System.Collections.Generic;
using System.Threading.Tasks;
using Otor.MsixHero.App.Helpers;
using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;
using Otor.MsixHero.Appx.Packaging.Registry;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Registry
{
    public class AppxRegistryViewModel : NotifyPropertyChanged
    {
        private readonly string packageFile;

        public AppxRegistryViewModel(string packageFile)
        {
            this.packageFile = packageFile;
            this.RootRegistries = new AsyncProperty<IList<AppxRegistryKeyViewModel>>(this.GetRoots());
            this.IsRegistryAvailable = this.CheckIfRegistryAvailable();
        }
        
        public bool IsRegistryAvailable { get; }
        
        public AsyncProperty<IList<AppxRegistryKeyViewModel>> RootRegistries { get; }
        
        public async Task<IList<AppxRegistryKeyViewModel>> GetRoots()
        {
            if (!this.CheckIfRegistryAvailable())
            {
                return null;
            }
            
            var roots = new List<AppxRegistryKeyViewModel>();

            using var appxReader = FileReaderFactory.CreateFileReader(this.packageFile);
            await using var registry = appxReader.GetFile("Registry.dat");
            using var reader = new AppxRegistryReader(registry);
            await foreach (var root in reader.EnumerateKeys(AppxRegistryRoots.Root).ConfigureAwait(false))
            {
                var rootViewModel = new AppxRegistryKeyViewModel(this, root);
                if (root.HasSubKeys)
                {
                    rootViewModel.Children.Add(null); // dummy
                }
                
                roots.Add(rootViewModel);
            }

            return roots;
        }

        private bool CheckIfRegistryAvailable()
        {
            using var reader = FileReaderFactory.CreateFileReader(this.packageFile);
            return reader.FileExists("registry.dat");
        }

        public IAppxFileReader GetAppxReader()
        {
            var reader = FileReaderFactory.CreateFileReader(this.packageFile);
            return reader;
        }
    }
}
