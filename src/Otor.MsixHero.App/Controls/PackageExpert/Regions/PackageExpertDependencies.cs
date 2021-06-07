using Otor.MsixHero.App.Controls.PackageExpert.Regions.Capabilities.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.General.Views;
using Prism.Ioc;
using Prism.Regions;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions
{
    public static class PackageExpertDependencies
    {
        public static string RegionGeneral = "PackageExpert_General";
        public static string RegionCapabilities = "PackageExpert_Capabilities";
        public static string RegionDependencies = "PackageExpert_Dependencies";

        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<CapabilitiesRegionView>();
            containerRegistry.RegisterSingleton<GeneralRegionView>();
            containerRegistry.RegisterSingleton<DependenciesRegionView>();
        }

        public static void RegisterRegions(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionCapabilities, typeof(CapabilitiesRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionGeneral, typeof(GeneralRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionDependencies, typeof(DependenciesRegionView));
        }
    }
}
