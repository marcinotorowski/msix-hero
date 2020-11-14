﻿using System;
using System.Windows;
using Otor.MsixHero.App.Hero;
using Otor.MsixHero.App.Hero.Commands;
using Otor.MsixHero.App.Hero.Executor;
using Otor.MsixHero.App.Hero.State;
using Otor.MsixHero.App.Modules;
using Otor.MsixHero.App.Modules.Editors;
using Otor.MsixHero.App.Modules.Editors.AppAttach;
using Otor.MsixHero.App.Modules.Editors.AppInstaller;
using Otor.MsixHero.App.Modules.Editors.Dependencies;
using Otor.MsixHero.App.Modules.Editors.Packaging;
using Otor.MsixHero.App.Modules.Editors.Signing;
using Otor.MsixHero.App.Modules.Editors.Updates;
using Otor.MsixHero.App.Modules.Editors.Winget;
using Otor.MsixHero.App.Modules.EventViewer;
using Otor.MsixHero.App.Modules.Main;
using Otor.MsixHero.App.Modules.Main.Shell.Views;
using Otor.MsixHero.App.Modules.Overview;
using Otor.MsixHero.App.Modules.Packages;
using Otor.MsixHero.App.Modules.SystemView;
using Otor.MsixHero.App.Modules.Volumes;
using Otor.MsixHero.App.Services;
using Otor.MsixHero.Appx.Diagnostic.Logging;
using Otor.MsixHero.Appx.Diagnostic.Recommendations;
using Otor.MsixHero.Appx.Diagnostic.Recommendations.ThirdParty;
using Otor.MsixHero.Appx.Diagnostic.Registry;
using Otor.MsixHero.Appx.Diagnostic.RunningDetector;
using Otor.MsixHero.Appx.Packaging.Installation;
using Otor.MsixHero.Appx.Packaging.ModificationPackages;
using Otor.MsixHero.Appx.Packaging.Packer;
using Otor.MsixHero.Appx.Signing;
using Otor.MsixHero.Appx.Updates;
using Otor.MsixHero.Appx.Volumes;
using Otor.MsixHero.Appx.WindowsVirtualDesktop.AppAttach;
using Otor.MsixHero.Dependencies;
using Otor.MsixHero.Infrastructure.Processes;
using Otor.MsixHero.Infrastructure.Processes.Ipc;
using Otor.MsixHero.Infrastructure.Processes.SelfElevation;
using Otor.MsixHero.Infrastructure.Services;
using Otor.MsixHero.Infrastructure.Updates;
using Otor.MsixHero.Lib.Infrastructure.Progress;
using Otor.MsixHero.Lib.Proxy;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Otor.MsixHero.App
{
    /// <summary>
    /// Interaction logic for the application.
    /// </summary>
    public partial class App : IDisposable
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IInteractionService, InteractionService>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<IAppxVolumeManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<IRegistryManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<ISigningManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<IAppxLogManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<IAppxPackageManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<ISelfElevationProxyProvider<IAppAttachManager>, SelfElevationManagerFactory>();
            containerRegistry.RegisterSingleton<IAppxPacker, AppxPacker>();
            containerRegistry.RegisterSingleton<IAppxContentBuilder, AppxContentBuilder>();
            containerRegistry.RegisterSingleton<IElevatedClient, Client>();
            containerRegistry.RegisterSingleton<IBusyManager, BusyManager>();
            containerRegistry.RegisterSingleton<IConfigurationService, LocalConfigurationService>();
            containerRegistry.RegisterSingleton<IUpdateChecker, HttpUpdateChecker>();
            containerRegistry.RegisterSingleton<IAppxVolumeManager, AppxVolumeManager>();
            containerRegistry.RegisterSingleton<IAppxPackageManager, AppxPackageManager>();
            containerRegistry.RegisterSingleton<IAppxUpdateImpactAnalyzer, AppxUpdateImpactAnalyzer>();
            containerRegistry.RegisterSingleton<IMsixHeroCommandExecutor, MsixHeroCommandExecutor>();
            containerRegistry.RegisterSingleton<IMsixHeroApplication, MsixHeroApplication>();
            containerRegistry.RegisterSingleton<IRunningDetector, RunningDetector>();
            containerRegistry.RegisterSingleton<IInterProcessCommunicationManager, InterProcessCommunicationManager>();
            containerRegistry.Register<IDependencyMapper, DependencyMapper>();
            containerRegistry.Register<IThirdPartyAppProvider, ThirdPartyAppProvider>();
            containerRegistry.Register<IServiceRecommendationAdvisor, ServiceRecommendationAdvisor>();
        }
        
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(new ModuleInfo(typeof(MainModule), ModuleNames.Main, InitializationMode.WhenAvailable));
            moduleCatalog.AddModule(new ModuleInfo(typeof(PackagesModule), ModuleNames.Packages, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(EventViewerModule), ModuleNames.EventViewer, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(SystemViewModule), ModuleNames.SystemView, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(VolumesModule), ModuleNames.Volumes, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(OverviewModule), ModuleNames.Overview, InitializationMode.OnDemand));
            
            moduleCatalog.AddModule(new ModuleInfo(typeof(SigningModule), DialogModuleNames.Signing, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(AppInstallerModule), DialogModuleNames.AppInstaller, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(PackagingModule), DialogModuleNames.Packaging, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(DependenciesModule), DialogModuleNames.Dependencies, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(UpdatesModule), DialogModuleNames.Updates, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(Modules.Editors.Volumes.VolumesModule), DialogModuleNames.Volumes, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(WingetModule), DialogModuleNames.Winget, InitializationMode.OnDemand));
            moduleCatalog.AddModule(new ModuleInfo(typeof(AppAttachModule), DialogModuleNames.AppAttach, InitializationMode.OnDemand));
            
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void Initialize()
        {
            base.Initialize();
            var regionManager = this.Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.Root, typeof(ShellView));


            var app = this.Container.Resolve<IMsixHeroApplication>();
            app.CommandExecutor.Invoke(null, new SetCurrentModeCommand(ApplicationMode.Overview));
        }

        protected override Window CreateShell()
        {
            var w = this.Container.Resolve<MainWindow>();
            return w;
        }

        public void Dispose()
        {
            var processManager = this.Container.Resolve<IInterProcessCommunicationManager>();
            processManager.Dispose();
        }
    }
}
