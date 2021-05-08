using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Otor.MsixHero.App.Mvvm;
using Otor.MsixHero.Appx.Packaging.Registry;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Registry
{
    public class AppxRegistryKeyViewModel : NotifyPropertyChanged
    {
        private readonly AppxRegistryViewModel parent;
        private bool isExpanded;
        private bool isExpandable;

        public AppxRegistryKeyViewModel(AppxRegistryViewModel parent, AppxRegistryKey model)
        {
            this.parent = parent;
            this.Path = model.Path;

            if (this.Path.Equals(AppxRegistryRoots.HKLM, StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "HKLM";
            }
            else if (this.Path.Equals(AppxRegistryRoots.HKCU, StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "HKCU";
            }
            else
            {
                this.Name = model.Path.Substring(model.Path.LastIndexOf('\\') + 1);
            }

            this.isExpandable = model.HasSubKeys;
        }

        public string Path { get; }

        public string Name { get; }

        public bool IsExpandable
        {
            get => this.isExpandable;
            set => this.SetField(ref this.isExpandable, value);
        }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set 
            {
                this.SetField(ref this.isExpanded, value);
                if (value)
                {
                    this.HandleExpansion();
                }
            }
        }

        private async void HandleExpansion()
        {
            this.Children.Clear();
            
            await foreach (var subKey in this.GetSubKeys().ConfigureAwait(true))
            {
                this.Children.Add(subKey);
            }
        }

        public ObservableCollection<AppxRegistryKeyViewModel> Children { get; private set; } = new ObservableCollection<AppxRegistryKeyViewModel>();

        public async IAsyncEnumerable<AppxRegistryKeyViewModel> GetSubKeys([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var reader = this.parent.GetAppxReader();
            if (!reader.FileExists("Registry.dat"))
            {
                yield break;
            }

            await using var registry = reader.GetFile("Registry.dat");
            using var appxRegistryReader = new AppxRegistryReader(registry);
            await foreach (var key in appxRegistryReader.EnumerateKeys(this.Path, cancellationToken))
            {
                var subKey = new AppxRegistryKeyViewModel(this.parent, key);
                if (key.HasSubKeys)
                {
                    subKey.Children.Add(null); // dummy element
                }
                
                yield return subKey;
            }
        }
        
        public async IAsyncEnumerable<AppxRegistryValueViewModel> GetValues([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var reader = this.parent.GetAppxReader();
            if (!reader.FileExists("Registry.dat"))
            {
                yield break;
            }

            await using var registry = reader.GetFile("Registry.dat");
            using var appxRegistryReader = new AppxRegistryReader(registry);
            
            await foreach (var value in appxRegistryReader.EnumerateValues(this.Path, cancellationToken))
            {
                yield return new AppxRegistryValueViewModel(value);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
