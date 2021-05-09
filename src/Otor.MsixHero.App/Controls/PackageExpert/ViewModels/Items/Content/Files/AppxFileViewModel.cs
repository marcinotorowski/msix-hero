namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Content.Files
{
    public class AppxFileViewModel : TreeNodeViewModel
    {
        public AppxFileViewModel(string fullPath)
        {
            this.Path = fullPath;
            this.Name = System.IO.Path.GetFileName(fullPath);
        }
    }
}
