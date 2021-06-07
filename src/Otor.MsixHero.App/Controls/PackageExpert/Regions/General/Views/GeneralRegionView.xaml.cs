using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Notifications.Wpf.Core;
using Otor.MsixHero.App.Services;
using Otor.MsixHero.Infrastructure.Helpers;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.General.Views
{
    /// <summary>
    /// Interaction logic for GeneralRegionView.xaml
    /// </summary>
    public partial class GeneralRegionView
    {
        public GeneralRegionView()
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
