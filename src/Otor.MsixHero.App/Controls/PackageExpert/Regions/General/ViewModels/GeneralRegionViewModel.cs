using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.App.Helpers;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;
using Otor.MsixHero.Appx.Signing;
using Otor.MsixHero.Appx.Signing.Entities;
using Otor.MsixHero.Infrastructure.Processes.SelfElevation;
using Otor.MsixHero.Infrastructure.Processes.SelfElevation.Enums;
using Otor.MsixHero.Infrastructure.Services;
using Prism.Commands;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.General.ViewModels
{
    public class GeneralRegionViewModel : PackageExpertPackageRegionViewModel
    {
        private readonly IInteractionService _interactionService;
        private readonly ISelfElevationProxyProvider<ISigningManager> _signManagerProvider;
        private ICommand _trustMe, _showPropertiesCommand;
        private bool _isTrusting;
        private string _certificateFile;

        public GeneralRegionViewModel(IInteractionService interactionService, ISelfElevationProxyProvider<ISigningManager> signManagerProvider)
        {
            _interactionService = interactionService;
            _signManagerProvider = signManagerProvider;
        }

        public string Publisher { get; private set; }
        public string Description { get; private set; }
        public string FamilyName { get; private set; }
        public string PackageFullName { get; private set; }
        public string Architecture { get; private set; }
        
        public bool HasBuildInfo { get; private set; }

        public string BuiltWithOperatingSystem { get; private set; }

        public bool PackageIntegrity { get; private set; }

        public string BuiltWithVersion { get; private set; }

        public string BuiltWith { get; private set; }

        public bool IsTrusting
        {
            get => this._isTrusting;
            set => this.SetField(ref this._isTrusting, value);
        }

        protected override async Task OnSourceChanged(AppxPackage sourcePackage)
        {
            this.HasBuildInfo = sourcePackage.BuildInfo != null;
            if (sourcePackage.BuildInfo != null)
            {
                this.BuiltWith = sourcePackage.BuildInfo.ProductName;
                this.BuiltWithOperatingSystem = sourcePackage.BuildInfo.OperatingSystem;
                this.BuiltWithVersion = sourcePackage.BuildInfo.ProductVersion;
            }

            this.PackageIntegrity = sourcePackage.PackageIntegrity;
            this.Architecture = sourcePackage.ProcessorArchitecture.ToString();
            this.Description = sourcePackage.Description;
            this.Publisher = sourcePackage.Publisher;
            this.FamilyName = sourcePackage.FamilyName;
            this.PackageFullName = sourcePackage.FullName;

            this.OnPropertyChanged(null);

            await this.LoadSignature(this.CurrentPackagePath);
        }

        public ICommand TrustMeCommand
        {
            get
            {
                return this._trustMe ??= new DelegateCommand(async () =>
                {
                    if (this._interactionService.Confirm("Are you sure you want to add this publisher to the list of trusted publishers (machine-wide)?", type: InteractionType.Question, buttons: InteractionButton.YesNo) != InteractionResult.Yes)
                    {
                        return;
                    }

                    try
                    {
                        this.IsTrusting = true;
                        
                        var manager = await this._signManagerProvider.GetProxyFor(SelfElevationLevel.AsAdministrator).ConfigureAwait(false);
                        await manager.Trust(this._certificateFile).ConfigureAwait(false);
                        await this.TrustStatus.Load(this.LoadSignature(this.CurrentPackagePath)).ConfigureAwait(false);
                    }
                    finally
                    {
                        this.IsTrusting = false;
                    }
                },
                    () => this._certificateFile != null);
            }
        }

        public ICommand ShowPropertiesCommand
        {
            get
            {
                return this._showPropertiesCommand ??= new DelegateCommand(() =>
                {
                    WindowsExplorerCertificateHelper.ShowFileSecurityProperties(this._certificateFile, IntPtr.Zero);
                }, () => this._certificateFile != null);
            }
        }

        public AsyncProperty<TrustStatus> TrustStatus { get; } = new AsyncProperty<TrustStatus>();

        public async Task<TrustStatus> LoadSignature(string package)
        {
            using var source = FileReaderFactory.CreateFileReader(package);
            if (source is ZipArchiveFileReaderAdapter zipFileReader)
            {
                this._certificateFile = zipFileReader.PackagePath;
                var manager = await this._signManagerProvider.GetProxyFor(SelfElevationLevel.AsInvoker).ConfigureAwait(false);
                var signTask = manager.IsTrusted(zipFileReader.PackagePath);
                await this.TrustStatus.Load(signTask);
                return await signTask.ConfigureAwait(false);
            }

            if (source is IAppxDiskFileReader fileReader)
            {
                var file = new FileInfo(Path.Combine(fileReader.RootDirectory, "AppxSignature.p7x"));
                this._certificateFile = file.FullName;
                if (file.Exists)
                {
                    var manager = await this._signManagerProvider.GetProxyFor(SelfElevationLevel.AsInvoker).ConfigureAwait(false);
                    var signTask = manager.IsTrusted(file.FullName);
                    await this.TrustStatus.Load(signTask);
                    return await signTask.ConfigureAwait(false);
                }
            }

            return null;
        }
    }
}
