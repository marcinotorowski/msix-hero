using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Notifications.Wpf.Core;
using Otor.MsixHero.App.Services;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Infrastructure.Helpers;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Installation.Views
{
    /// <summary>
    /// Interaction logic for InstallationRegionView.xaml
    /// </summary>
    public partial class InstallationRegionView
    {
        public InstallationRegionView()
        {
            this.InitializeComponent();
        }

        private void OpenManifest(object sender, RoutedEventArgs e)
        {
            ExceptionGuard.Guard(() =>
                {
                    var dir = (string)((Hyperlink)sender).Tag;
                    Process.Start("explorer.exe", "/select," + Path.Combine(dir, FileConstants.AppxManifestFile));
                },
                new InteractionService(new NotificationManager()));
        }
    }
}
