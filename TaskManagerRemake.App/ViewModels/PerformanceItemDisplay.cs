using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;
using TaskManagerRemake.Domain.Services.PerformanceTab;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceItemDisplay
    {
        private readonly IPerformanceItem selectedTab;
        public string TabTitle { get; set; }
        public string TabSpec { get; set; }
        public ObservableCollection<PerformanceStat> StaticStats { get; set; }
        // private ObservableCollection<PerformanceStat> _dynamicStats;

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
        }
    }
}
