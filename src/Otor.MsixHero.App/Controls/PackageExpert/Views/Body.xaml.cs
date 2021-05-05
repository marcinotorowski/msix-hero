// MSIX Hero
// Copyright (C) 2021 Marcin Otorowski
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// Full notice:
// https://github.com/marcinotorowski/msix-hero/blob/develop/LICENSE.md

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Notifications.Wpf.Core;
using Otor.MsixHero.App.Controls.PackageExpert.ViewModels;
using Otor.MsixHero.App.Controls.PackageExpert.ViewModels.Items.Registry;
using Otor.MsixHero.App.Services;
using Otor.MsixHero.Appx.Packaging;
using Otor.MsixHero.Infrastructure.Helpers;

namespace Otor.MsixHero.App.Controls.PackageExpert.Views
{
    /// <summary>
    /// Interaction logic for PackageExpert
    /// </summary>
    public partial class Body
    {
        public Body()
        {
            this.InitializeComponent();
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

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            ExceptionGuard.Guard(() =>
            {
                var dir = (string)((Hyperlink) sender).Tag;
                Process.Start("explorer.exe", "/select," + Path.Combine(dir, FileConstants.AppxManifestFile));
            }, 
            new InteractionService(new NotificationManager()));
        }

        private async void OnRegistryExpanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem) e.OriginalSource;
            treeViewItem.Items.Clear();

            var parentRegistry = (AppxRegistryKeyViewModel) treeViewItem.Tag;
                                          
            await foreach (var reg in parentRegistry.GetSubKeys().ConfigureAwait(true))
            {
                var tv = new TreeViewItem();
                if (reg.IsExpandable)
                {
                    tv.Items.Add("placeholder");
                }

                tv.Tag = reg;
                tv.Header = reg.Name;
                treeViewItem.Items.Add(tv);
            }
        }
        
        private async void OnRegistrySelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = (TreeView) sender;
            this.Values.Items.Clear();
            var selectedPath = treeView.SelectedItem as TreeViewItem;
            if (selectedPath == null)
            {
                this.Values.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Values.Visibility = Visibility.Visible;
                var parentKey = (AppxRegistryKeyViewModel) selectedPath.Tag;
                await foreach (var reg in parentKey.GetValues().ConfigureAwait(true))
                {
                    this.Values.Items.Add(reg);
                }
            }
        }

        private async void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = (PackageExpertViewModel) e.NewValue;
            
            if (dataContext == null)
            {
                ((TreeView)sender).Items.Clear();
                return;
            }

            var registry = await dataContext.Registry.GetRoots().ConfigureAwait(true);
            if (registry == null)
            {
                return;
            }

            this.Values.Visibility = Visibility.Collapsed;
            var treeView = (TreeView)sender;
            treeView.Items.Clear();

            foreach (var item in registry)
            {
                var tv = new TreeViewItem();
                if (item.IsExpandable)
                {
                    tv.Items.Add("placeholder");
                }

                tv.Tag = item;
                tv.Header = item.Name;
                treeView.Items.Add(tv);
            }
        }
    }
}
