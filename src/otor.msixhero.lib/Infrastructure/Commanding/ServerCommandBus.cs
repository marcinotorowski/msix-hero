﻿using otor.msixhero.lib.BusinessLayer.Executors.Client;
using otor.msixhero.lib.BusinessLayer.Executors.General;
using otor.msixhero.lib.BusinessLayer.Executors.Server;
using otor.msixhero.lib.BusinessLayer.Managers.AppAttach;
using otor.msixhero.lib.BusinessLayer.Managers.Packages;
using otor.msixhero.lib.BusinessLayer.Managers.Registry;
using otor.msixhero.lib.BusinessLayer.Managers.Signing;
using otor.msixhero.lib.BusinessLayer.Managers.Volumes;
using otor.msixhero.lib.Domain.Commands.Packages.AppAttach;
using otor.msixhero.lib.Domain.Commands.Packages.Developer;
using otor.msixhero.lib.Domain.Commands.Packages.Grid;
using otor.msixhero.lib.Domain.Commands.Packages.Manager;
using otor.msixhero.lib.Domain.Commands.Packages.Signing;
using otor.msixhero.lib.Domain.Commands.Volumes;
using otor.msixhero.lib.Infrastructure.Ipc;
using otor.msixhero.lib.Infrastructure.SelfElevation;

namespace otor.msixhero.lib.Infrastructure.Commanding
{
    public class ServerCommandBus : CommandBus
    {
        public ServerCommandBus(
            IInteractionService interactionService,
            IElevatedClient elevatedClient,
            ISelfElevationManagerFactory<IAppxVolumeManager> volumeManagerFactory,
            ISelfElevationManagerFactory<IAppxPackageManager> packageManagerFactory,
            ISelfElevationManagerFactory<IRegistryManager> registryManagerFactory,
            ISelfElevationManagerFactory<IAppAttachManager> appAttachManagerFactory,
            ISelfElevationManagerFactory<ISigningManager> signingManagerFactory) : base(interactionService, elevatedClient, volumeManagerFactory, packageManagerFactory, registryManagerFactory, appAttachManagerFactory, signingManagerFactory)
        {
        }

        protected override void RegisterReducers()
        {
            this.CommandExecutorFactories[typeof(GetPackages)] = action => new GetInstalledPackagesServerExecutor((GetPackages)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(GetVolumes)] = action => new GetVolumesServerExecutor((GetVolumes)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(RemovePackages)] = action => new RemovePackageServerExecutor((RemovePackages)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(RemoveVolume)] = action => new RemoveVolumeServerExecutor((RemoveVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);

            this.CommandExecutorFactories[typeof(GetRegistryMountState)] = action => new GetRegistryMountStateCommandExecutor((GetRegistryMountState)action, this.RegistryManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(RunPackage)] = action => new RunPackageCommandExecutor((RunPackage)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(RunToolInPackage)] = action => new RunToolInPackageCommandExecutor((RunToolInPackage)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(ConvertToVhd)] = action => new ConvertToVhdCommandExecutor((ConvertToVhd)action, this.AppAttachManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(Deprovision)] = action => new DeprovisionCommandExecutor((Deprovision)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(FindUsers)] = action => new FindUsersCommandExecutor((FindUsers)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(MountRegistry)] = action => new MountRegistryCommandExecutor((MountRegistry)action, this.RegistryManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(DismountRegistry)] = action => new DismountRegistryCommandExecutor((DismountRegistry)action, this.RegistryManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(GetLogs)] = action => new GetLogsCommandExecutor((GetLogs)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(AddPackage)] = action => new AddPackageCommandExecutor((AddPackage)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(GetPackageDetails)] = action => new GetPackageDetailsCommandExecutor((GetPackageDetails)action, this.PackageManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(InstallCertificate)] = action => new InstallCertificateCommandExecutor((InstallCertificate)action, this.SigningManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(AddVolume)] = action => new AddVolumeCommandExecutor((AddVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(SetDefaultVolume)] = action => new SetDefaultVolumeCommandExecutor((SetDefaultVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(DismountVolume)] = action => new DismountVolumeCommandExecutor((DismountVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(MountVolume)] = action => new MountVolumeCommandExecutor((MountVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
            this.CommandExecutorFactories[typeof(ChangeVolume)] = action => new ChangeVolumeCommandExecutor((ChangeVolume)action, this.VolumeManagerFactory, this.WritableApplicationStateManager);
        }
    }
}
