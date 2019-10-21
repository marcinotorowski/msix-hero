﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSI_Hero.Domain;
using MSI_Hero.Domain.Actions;
using MSI_Hero.Domain.Events;
using MSI_Hero.Domain.State.Enums;
using MSI_Hero.ViewModel;
using otor.msihero.lib;
using Prism;
using Prism.Events;
using Prism.Regions;

namespace MSI_Hero.Modules.Installed.ViewModel
{
    public class InstalledViewModel : NotifyPropertyChanged, INavigationAware, IActiveAware
    {
        private readonly IApplicationStateManager stateManager;
        private PackageViewModel selectedPackage;
        private bool isActive;

        public InstalledViewModel(IApplicationStateManager stateManager)
        {
            this.stateManager = stateManager;

            this.AllPackages = new ObservableCollection<PackageViewModel>();
            stateManager.EventAggregator.GetEvent<PackagesFilterChanged>().Subscribe(this.OnPackageFilterChanged, ThreadOption.UIThread);
            stateManager.EventAggregator.GetEvent<PackagesLoadedEvent>().Subscribe(this.OnPackagesLoaded, ThreadOption.UIThread);
            stateManager.EventAggregator.GetEvent<PackagesVisibilityChanged>().Subscribe(this.OnPackagesVisibilityChanged, ThreadOption.UIThread);
            stateManager.EventAggregator.GetEvent<PackagesSelectionChanged>().Subscribe(this.OnPackagesSelectionChanged, ThreadOption.UIThread);
        }

        private void OnPackagesSelectionChanged(PackagesSelectionChangedPayLoad selectionInfo)
        {
            this.selectedPackage = this.AllPackages.FirstOrDefault(app => this.stateManager.CurrentState.Packages.SelectedItems.Contains(app.Model));
            this.OnPropertyChanged(nameof(SelectedPackage));
            this.OnPropertyChanged(nameof(IsSelected));
        }

        private void OnPackagesVisibilityChanged(PackagesVisibilityChangedPayLoad visibilityInfo)
        {
            for (var i = this.AllPackages.Count - 1; i >= 0 ; i--)
            {
                var item = this.AllPackages[i];
                if (visibilityInfo.NewHidden.Contains(item.Model))
                {
                    this.AllPackages.RemoveAt(i);
                }
            }

            foreach (var item in visibilityInfo.NewVisible)
            {
                this.AllPackages.Add(new PackageViewModel(item));
            }
        }

        private void OnPackagesLoaded(PackageContext context)
        {
            this.AllPackages.Clear();
            foreach (var item in this.stateManager.CurrentState.Packages.VisibleItems)
            {
                this.AllPackages.Add(new PackageViewModel(item));
            }
        }

        private void OnPackageFilterChanged(PackagesFilterChangedPayload filter)
        {
            if (filter.NewSearchKey != filter.OldSearchKey)
            {
                this.OnPropertyChanged(nameof(SearchKey));
            }
        }
        
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
                var iac = this.IsActiveChanged;
                if (iac != null)
                {
                    iac(this, new EventArgs());
                }

#pragma warning disable 4014
                this.RefreshPackages();
#pragma warning restore 4014
            }
        }

        public event EventHandler IsActiveChanged;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public string SearchKey
        {
            get => this.stateManager.CurrentState.Packages.SearchKey;
            set => this.stateManager.Executor.ExecuteAsync(SetPackageFilter.CreateFrom(value));
        }

        public ObservableCollection<PackageViewModel> AllPackages { get; }
        
        public PackageViewModel SelectedPackage
        {
            get => this.selectedPackage;
            set
            {
                if (value == null)
                {
                    this.stateManager.Executor.ExecuteAsync(SelectPackages.CreateEmpty());
                }
                else
                {
                    this.stateManager.Executor.ExecuteAsync(new SelectPackages(value.Model));
                }
            }
        }

        public bool IsSelected
        {
            get => this.selectedPackage != null;
        }
        
        public Task RefreshPackages(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.stateManager.Executor.ExecuteAsync(new ReloadPackages(), cancellationToken);
        }
    }
}