using Otor.MsixHero.App.Controls.PackageExpert.Regions.Applications.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Capabilities.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Files.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.General.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Installation.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.Views;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Registry.Views;
using Prism.Ioc;
using Prism.Regions;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions
{
    public static class PackageExpertDependencies
    {
        public static string RegionGeneral = "PackageExpert_General";
        public static string RegionCapabilities = "PackageExpert_Capabilities";
        public static string RegionDependencies = "PackageExpert_Dependencies";
        public static string RegionInstallation = "PackageExpert_Installation";
        public static string RegionApplications = "PackageExpert_Applications";
        public static string RegionFiles = "PackageExpert_Files";
        public static string RegionRegistry = "PackageExpert_Registry";
        public static string RegionPsf = "PackageExpert_Psf";

        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public static void RegisterRegions(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionCapabilities, typeof(CapabilitiesRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionGeneral, typeof(GeneralRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionDependencies, typeof(DependenciesRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionApplications, typeof(ApplicationsRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionRegistry, typeof(RegistryRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionFiles, typeof(FilesRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionInstallation, typeof(InstallationRegionView));
            regionManager.RegisterViewWithRegion(PackageExpertDependencies.RegionPsf, typeof(PsfRegionView));
        }
    }
}
