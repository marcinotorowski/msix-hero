using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Notifications.Wpf.Core;
using Otor.MsixHero.App.Services;
using Otor.MsixHero.Infrastructure.Helpers;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Dependencies.Views
{
    /// <summary>
    /// Interaction logic for DependenciesRegionView.xaml
    /// </summary>
    public partial class DependenciesRegionView
    {
        public DependenciesRegionView()
        {
            InitializeComponent();
        }

        private void HyperlinkOnClick(object sender, RoutedEventArgs e)
        {
            ExceptionGuard.Guard(() =>
                {
                    var psi = new ProcessStartInfo((string)((Hyperlink)sender).Tag)
                    {
                        UseShellExecute = true
                    };

                    Process.Start(psi);
                },
                new InteractionService(new NotificationManager()));
        }
    }
}
