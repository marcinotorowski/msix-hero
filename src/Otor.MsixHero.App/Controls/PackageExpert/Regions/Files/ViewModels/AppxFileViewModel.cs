using Otor.MsixHero.App.Controls.PackageExpert.Regions.General;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Files.ViewModels
{
    public class AppxFileViewModel : TreeNodeViewModel
    {
        public AppxFileViewModel(AppxFileInfo fileInfo)
        {
            this.Path = fileInfo.FullPath;
            this.Name = fileInfo.Name;
            this.Size = fileInfo.Size;
        }
        
        public long Size { get; }
    }
}
