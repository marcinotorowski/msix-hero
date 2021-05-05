using Otor.MsixHero.Appx.Packaging.Registry;

namespace Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Registry
{
    public class AppxRegistryValueViewModel
    {
        public AppxRegistryValueViewModel(AppxRegistryValue value)
        {
            this.Name = value.Name;
            
            if (value.Type != "RegNone")
            {
                this.Type = value.Type;
                this.Data = value.Data;
            }
        }
        
        public string Name { get; }
        
        public string Type { get; }
        
        public string Data { get; }
    }
}
