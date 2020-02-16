﻿using System;
using System.Linq;
using System.Threading.Tasks;
using otor.msixhero.lib.BusinessLayer.Appx.Signing;
using otor.msixhero.lib.Domain.Events;
using otor.msixhero.lib.Infrastructure;
using otor.msixhero.lib.Infrastructure.Configuration;
using otor.msixhero.ui.Domain;
using otor.msixhero.ui.Modules.Dialogs.Common.CertificateSelector.ViewModel;
using otor.msixhero.ui.Modules.Settings.ViewModel.Tools;
using Prism.Events;
using Prism.Services.Dialogs;

namespace otor.msixhero.ui.Modules.Settings.ViewModel
{
    public class SettingsViewModel : IDialogAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationService configurationService;

        public SettingsViewModel(
            IEventAggregator eventAggregator,
            IConfigurationService configurationService, 
            IInteractionService interactionService, 
            IAppxSigningManager signingManager)
        {
            this.eventAggregator = eventAggregator;
            this.configurationService = configurationService;

            var config = configurationService.GetCurrentConfiguration() ?? new Configuration();
            
            this.AllSettings.AddChildren(
                this.CertificateOutputPath = new ChangeableFolderProperty(interactionService, config.Signing?.DefaultOutFolder?.Resolved),
                this.PackerSignByDefault = new ChangeableProperty<bool>(config.Packer?.SignByDefault == true),
                this.SidebarDefaultState = new ChangeableProperty<bool>(config.List.Sidebar.Visible),
                this.CertificateSelector = new CertificateSelectorViewModel(interactionService, signingManager, config.Signing, false),
                this.ManifestEditorType = new ChangeableProperty<EditorType>(config.Editing.ManifestEditorType),
                this.ManifestEditorPath = new ChangeableFileProperty(interactionService, config.Editing.ManifestEditor.Resolved),
                this.MsixEditorType = new ChangeableProperty<EditorType>(config.Editing.ManifestEditorType),
                this.MsixEditorPath = new ChangeableFileProperty(interactionService, config.Editing.MsixEditor.Resolved),
                this.AppinstallerEditorType = new ChangeableProperty<EditorType>(config.Editing.AppInstallerEditorType),
                this.AppinstallerEditorPath = new ChangeableFileProperty(interactionService, config.Editing.AppInstallerEditor.Resolved),
                this.PsfEditorType = new ChangeableProperty<EditorType>(config.Editing.PsfEditorType),
                this.PsfEditorPath = new ChangeableFileProperty(interactionService, config.Editing.PsfEditor.Resolved),
                this.DefaultRemoteLocationPackages = new ValidatedChangeableProperty<string>(config.AppInstaller?.DefaultRemoteLocationPackages, this.ValidateUri),
                this.DefaultRemoteLocationAppInstaller = new ValidatedChangeableProperty<string>(config.AppInstaller?.DefaultRemoteLocationAppInstaller, this.ValidateUri),
                this.Tools = new ToolsConfigurationViewModel(interactionService, config)
            );

            this.CertificateOutputPath.Validators = new[] { ChangeableFolderProperty.ValidatePath };
        }
        
        public ToolsConfigurationViewModel Tools { get; }

        public CertificateSelectorViewModel CertificateSelector { get; }

        public ChangeableContainer AllSettings { get; } = new ChangeableContainer();

        public ChangeableFolderProperty CertificateOutputPath { get; }
        
        public ValidatedChangeableProperty<string> DefaultRemoteLocationPackages { get; }

        public ValidatedChangeableProperty<string> DefaultRemoteLocationAppInstaller { get; }

        public ChangeableProperty<EditorType> ManifestEditorType { get; }

        public ChangeableFileProperty ManifestEditorPath { get; }

        public ChangeableProperty<EditorType> MsixEditorType { get; }

        public ChangeableFileProperty MsixEditorPath { get; }

        public ChangeableProperty<EditorType> AppinstallerEditorType { get; }

        public ChangeableFileProperty AppinstallerEditorPath { get; }

        public ChangeableProperty<EditorType> PsfEditorType { get; }

        public ChangeableFileProperty PsfEditorPath { get; }

        public ChangeableProperty<bool> PackerSignByDefault { get; }
        
        public ChangeableProperty<bool> SidebarDefaultState { get; }

        public string Title => "Settings";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public bool CanSave()
        {
            return this.AllSettings.IsTouched && (!this.AllSettings.IsValidated || this.AllSettings.IsValid);
        }
        
        public async Task<bool> Save()
        {
            if (!this.AllSettings.IsValidated)
            {
                this.AllSettings.IsValidated = true;
            }

            if (!this.AllSettings.IsValid)
            {
                return false;
            }

            var newConfiguration = this.configurationService.GetCurrentConfiguration(false);

            if (this.CertificateOutputPath.IsTouched)
            {
                newConfiguration.Signing.DefaultOutFolder = this.CertificateOutputPath.CurrentValue;
            }

            if (this.PackerSignByDefault.IsTouched)
            {
                newConfiguration.Packer.SignByDefault = this.PackerSignByDefault.CurrentValue;
            }
            
            if (this.SidebarDefaultState.IsTouched)
            {
                newConfiguration.List.Sidebar.Visible = this.SidebarDefaultState.CurrentValue;
            }

            if (this.CertificateSelector.IsTouched)
            {
                if (this.CertificateSelector.PfxPath.IsTouched)
                {
                    newConfiguration.Signing.PfxPath.Resolved = this.CertificateSelector.PfxPath.CurrentValue;
                }

                if (this.CertificateSelector.Store.IsTouched)
                {
                    newConfiguration.Signing.Source = this.CertificateSelector.Store.CurrentValue;
                }

                if (this.CertificateSelector.SelectedPersonalCertificate.IsTouched)
                {
                    newConfiguration.Signing.Thumbprint = this.CertificateSelector.SelectedPersonalCertificate.CurrentValue?.Model?.Thumbprint;
                }

                if (this.CertificateSelector.TimeStamp.IsTouched)
                {
                    newConfiguration.Signing.TimeStampServer = this.CertificateSelector.TimeStamp.CurrentValue;
                }
            }

            if (this.ManifestEditorType.IsTouched)
            {
                newConfiguration.Editing.ManifestEditorType = this.ManifestEditorType.CurrentValue;
            }

            if (this.AppinstallerEditorType.IsTouched)
            {
                newConfiguration.Editing.AppInstallerEditorType = this.AppinstallerEditorType.CurrentValue;
            }

            if (this.MsixEditorType.IsTouched)
            {
                newConfiguration.Editing.MsixEditorType = this.MsixEditorType.CurrentValue;
            }

            if (this.PsfEditorType.IsTouched)
            {
                newConfiguration.Editing.PsfEditorType = this.PsfEditorType.CurrentValue;
            }

            if (this.ManifestEditorPath.IsTouched)
            {
                newConfiguration.Editing.ManifestEditor.Resolved = this.ManifestEditorPath.CurrentValue;
            }

            if (this.AppinstallerEditorPath.IsTouched)
            {
                newConfiguration.Editing.AppInstallerEditor.Resolved = this.AppinstallerEditorPath.CurrentValue;
            }

            if (this.MsixEditorPath.IsTouched)
            {
                newConfiguration.Editing.MsixEditor.Resolved = this.MsixEditorPath.CurrentValue;
            }

            if (this.PsfEditorPath.IsTouched)
            {
                newConfiguration.Editing.PsfEditor.Resolved = this.PsfEditorPath.CurrentValue;
            }

            var toolsTouched = this.Tools.IsTouched;
            this.AllSettings.Commit();

            if (toolsTouched)
            {
                newConfiguration.List.Tools.Clear();
                newConfiguration.List.Tools.AddRange(this.Tools.Items.Select(t => (ToolListConfiguration)t));
            }

            await this.configurationService.SetCurrentConfigurationAsync(newConfiguration).ConfigureAwait(false);

            if (toolsTouched)
            {
                this.eventAggregator.GetEvent<ToolsChangedEvent>().Publish(this.Tools.Items.Select(t => (ToolListConfiguration)t).ToArray());
            }

            return true;
        }

        public event Action<IDialogResult> RequestClose;

        private string ValidateUri(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            {
                return $"The value '{value}' is not a valid URI.";
            }

            return null;
        }
    }
}
