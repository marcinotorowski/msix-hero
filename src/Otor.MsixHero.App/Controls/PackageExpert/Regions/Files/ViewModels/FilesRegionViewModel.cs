using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;
using Otor.MsixHero.Infrastructure.Helpers;
using Otor.MsixHero.Infrastructure.Services;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Files.ViewModels
{
    public class FilesRegionViewModel : PackageExpertPackageRegionViewModel
    {
        private readonly FileInvoker fileInvoker;
        private readonly IAppxFileViewer fileViewer;

        public FilesRegionViewModel(IAppxFileViewer fileViewer, IInteractionService interactionService, IConfigurationService configurationService)
        {
            this.fileViewer = fileViewer;
            this.fileInvoker = new FileInvoker(interactionService, configurationService);
        }

        protected override Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.FileTree = new AppxFilesViewModel(sourcePackage.PackageFile, this.fileViewer, this.fileInvoker);
            this.OnPropertyChanged(nameof(this.FileTree));
            return Task.CompletedTask;
        }

        public AppxFilesViewModel FileTree { get; private set; }
    }
}
