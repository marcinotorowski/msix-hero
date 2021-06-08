using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Common;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels.Items.Custom;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels.Items.Electron;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels.Items.Redirection;
using Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels.Items.Trace;
using Otor.MsixHero.App.Helpers;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Appx.Packaging.Installation.Enums;
using Otor.MsixHero.Appx.Packaging.Manifest.Entities;
using Otor.MsixHero.Appx.Packaging.Manifest.FileReaders;
using Otor.MsixHero.Appx.Psf.Entities;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Psf.ViewModels
{
    public class PsfRegionViewModel : PackageExpertPackageRegionViewModel
    {
        private IEnumerable<string> GetRootPathsForConfigJson(AppxPackage sourcePackage)
        {
            foreach (var elem in sourcePackage.Applications
                .Where(a => PackageTypeConverter.GetPackageTypeFrom(a.EntryPoint, a.Executable, a.StartPage, sourcePackage.IsFramework) == MsixPackageType.BridgePsf).Select(a => a.Executable)
                .Where(a => a != null).Select(Path.GetDirectoryName).Where(a => !string.IsNullOrEmpty(a)).Distinct())
            {
                yield return elem;
            }

            yield return string.Empty;
        }

        protected override async Task OnSourceChanged(AppxPackage sourcePackage)
        {
            try
            {
                this.Progress.Progress = 0;
                this.Progress.IsLoading = true;
                this.Progress.Error = null;
                this.HasPsf = false;
                this.RedirectionRules = new List<PsfContentProcessRedirectionViewModel>();
                this.TraceRules = new List<PsfContentProcessTraceViewModel>();
                this.CustomRules = new List<PsfContentProcessCustomViewModel>();
                this.ElectronRules = new List<PsfContentProcessElectronViewModel>();

                using var appxReader = FileReaderFactory.CreateFileReader(sourcePackage.PackageFile);

                foreach (var items in this.GetRootPathsForConfigJson(sourcePackage))
                {
                    var configJsonPath = string.IsNullOrWhiteSpace(items) ? "config.json" : Path.Combine(items, "config.json");
                    if (!appxReader.FileExists(configJsonPath))
                    {
                        continue;
                    }

                    await using var jsonStream = appxReader.GetFile(configJsonPath);
                    using var stringReader = new StreamReader(jsonStream);
                    var all = await stringReader.ReadToEndAsync().ConfigureAwait(false);
                    var psfSerializer = new PsfConfigSerializer();

                    var configJson = psfSerializer.Deserialize(all);
                    this.Setup(configJson);
                    break;
                }

                this.OnPropertyChanged(null);
            }
            catch (Exception e)
            {
                this.Progress.Error = e.Message;
                throw;
            }
            finally
            {
                this.Progress.Progress = 100;
                this.Progress.IsLoading = false;
                
            }
        }

        public ProgressProperty Progress { get; } = new ProgressProperty();

        public IList<PsfContentProcessRedirectionViewModel> RedirectionRules { get; private set; }

        public IList<PsfContentProcessTraceViewModel> TraceRules { get; private set; }

        public IList<PsfContentProcessElectronViewModel> ElectronRules { get; private set; }

        public IList<PsfContentProcessCustomViewModel> CustomRules { get; private set; }

        public bool HasTraceRules => this.TraceRules.Any();

        public bool HasRedirectionRules => this.RedirectionRules.Any();

        public bool HasElectronRules => this.RedirectionRules.Any();

        public bool HasCustomRules => this.CustomRules.Any();

        public bool HasPsf { get; private set; }

        private void Setup(PsfConfig psfConfig)
        {
            if (psfConfig?.Processes != null)
            {
                foreach (var process in psfConfig.Processes)
                {
                    foreach (var item in process.Fixups.Where(f => f.Config != null))
                    {
                        if (item.Config is PsfRedirectionFixupConfig redirectionConfig)
                        {
                            var psfContentProcessViewModel = new PsfContentProcessRedirectionViewModel(process.Executable, item.Dll, redirectionConfig.RedirectedPaths);
                            this.RedirectionRules.Add(psfContentProcessViewModel);
                            this.HasPsf = true;
                        }
                        else if (item.Config is PsfTraceFixupConfig traceConfig)
                        {
                            var psfContentProcessViewModel = new PsfContentProcessTraceViewModel(process.Executable, item.Dll, traceConfig);
                            this.TraceRules.Add(psfContentProcessViewModel);
                            this.HasPsf = true;
                        }
                        else if (item.Config is PsfElectronFixupConfig electronConfig)
                        {
                            var psfContentProcessViewModel = new PsfContentProcessElectronViewModel(process.Executable, item.Dll, electronConfig);
                            this.ElectronRules.Add(psfContentProcessViewModel);
                            this.HasPsf = true;
                        }
                        else if (item.Config is CustomPsfFixupConfig customConfig)
                        {
                            var psfContentProcessViewModel = new PsfContentProcessCustomViewModel(process.Executable, item.Dll, customConfig);
                            this.CustomRules.Add(psfContentProcessViewModel);
                            this.HasPsf = true;
                        }
                    }
                }
            }

            this.OnPropertyChanged(null);
        }
    }
}
