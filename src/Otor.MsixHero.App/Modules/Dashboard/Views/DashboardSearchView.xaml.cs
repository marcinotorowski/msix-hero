﻿using System.Windows;
using System.Windows.Input;

namespace Otor.MsixHero.App.Modules.Dashboard.Views
{
    public partial class DashboardSearchView
    {
        public DashboardSearchView()
        {
            InitializeComponent();
        }

        private void ClearSearchField(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Text = string.Empty;
            this.SearchBox.Focus();
            FocusManager.SetFocusedElement(this, this.SearchBox);
            Keyboard.Focus(this.SearchBox);
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SearchBox.Focus();
            Keyboard.Focus(this.SearchBox);
        }
    }
}