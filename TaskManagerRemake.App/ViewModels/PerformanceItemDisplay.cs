using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
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
        
        // Chart properties
        public ChartValues<int> _lineChartValues;

        public PerformanceItemDisplay(string tabToDisplay)
        {
            if (tabToDisplay == "cpu") selectedTab = new CpuTab();
            else if (tabToDisplay == "memory") selectedTab = new MemoryTab();

            InitializeTab();
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

        public ChartValues<int> LineChartValues
        {
            get => _lineChartValues;
            set
            {
                _lineChartValues = value;
                OnPropertyChanged(nameof(LineChartValues));
            }
        }

        private void InitializeTab()
        {
            TabTitle = selectedTab.GetTabTitle();
            StaticStats = new ObservableCollection<PerformanceStat>(selectedTab.GetStaticStats());
            TabSpec = selectedTab.GetTabSpecs();
            DynamicStats = new ObservableCollection<PerformanceStat>(selectedTab.GetDynamicStats());

            // For testing purposes - will probably need a singleton or similar
            if (selectedTab.GetType() == typeof(CpuTab))
            {
                LineChartValues = new ChartValues<int>();

                UpdateLineChart();

                InitTimer();
            }
            if (selectedTab.GetType() == typeof(MemoryTab))
            {
                LineChartValues = new ChartValues<int>();

                UpdateLineChart();

                InitTimer();
            }
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
            Debug.WriteLine("timer tick BEFORE");
            DynamicStats = new ObservableCollection<PerformanceStat>(selectedTab.GetDynamicStats());
            Debug.WriteLine("timer tick AFTER");
            UpdateLineChart();

            CommandManager.InvalidateRequerySuggested();
        }

        private void UpdateLineChart()
        {
            var data = selectedTab.GetDataForChart();
            LineChartValues.Add(data);

            if (LineChartValues.Count > 61)
            {
                LineChartValues.RemoveAt(0);
            }
        }
    }
}
