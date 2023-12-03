using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TaskManagerRemake.Domain.Models;
using TaskManagerRemake.Domain.Services.PerformanceTab;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceItemDisplay : ViewModelBase
    {
        private readonly IPerformanceItem selectedTab;
        public string TabTitle { get; set; }
        public string TabSpec { get; set; }
        public ObservableCollection<PerformanceStat> StaticStats { get; set; }
        public ObservableCollection<PerformanceStat> _dynamicStats;

        public PerformanceItemDisplay(string tabToDisplay)
        {
            if (tabToDisplay == "cpu") selectedTab = new CpuTab();
            else if (tabToDisplay == "memory") selectedTab = new MemoryTab();

            InitializeTab();
        }

        private void InitializeTab()
        {
            TabTitle = selectedTab.GetTabTitle();
            StaticStats = new ObservableCollection<PerformanceStat>(selectedTab.GetStaticStats());
            TabSpec = selectedTab.GetTabSpecs();
            DynamicStats = new ObservableCollection<PerformanceStat>(selectedTab.GetDynamicStats());

            InitTimer();
        }

        private void InitTimer()
        {
            DispatcherTimer usageTimer = new DispatcherTimer();

            usageTimer.Tick += new EventHandler(Timer_Tick);
            usageTimer.Interval = TimeSpan.FromSeconds(1);
            usageTimer.Start();
        }

        private void Timer_Tick(Object source, EventArgs e)
        {
            DynamicStats = new ObservableCollection<PerformanceStat>(selectedTab.GetDynamicStats());

            CommandManager.InvalidateRequerySuggested();
        }

        public ObservableCollection<PerformanceStat> DynamicStats
        {
            get => _dynamicStats;
            set
            {
                _dynamicStats = value;
                OnPropertyChanged(nameof(DynamicStats));
            }
        }
    }
}
