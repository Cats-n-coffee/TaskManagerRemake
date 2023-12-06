using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;
using TaskManagerRemake.Domain.Services.PerformanceTab;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Controls.DataVisualization.Charting;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceViewModel : ViewModelBase
    {
        public ObservableCollection<PerformanceItemDisplay> PerformanceItems { get; set; }
        // private ObservableCollection<PerformanceStat> _dynamicStats;
        public Dictionary<string, int> FakeData { get; set; }
        public PerformanceViewModel()
        {
            FakeData = new Dictionary<string, int>
            {
                { "aaa", 34 },
                { "ggg", 23 },
                { "ppp", 12 },
                { "oo", 3 },
                { "ss", 4 }
            };

            PerformanceItems = new ObservableCollection<PerformanceItemDisplay>(
                new List<PerformanceItemDisplay>() 
                {
                    new PerformanceItemDisplay("cpu"),
                    new PerformanceItemDisplay("memory"),
                }
                );
            /*

            InitTimer();

            DynamicStats = new ObservableCollection<PerformanceStat>(performanceItem.GetDynamicStats());*/
        }
        /*
        private void InitTimer()
        {
            DispatcherTimer usageTimer = new DispatcherTimer();

            usageTimer.Tick += new EventHandler(Timer_Tick);
            usageTimer.Interval = TimeSpan.FromSeconds(1);
            usageTimer.Start();
        }

        private void Timer_Tick(Object source, EventArgs e)
        {
            DynamicStats = new ObservableCollection<PerformanceStat>(performanceItem.GetDynamicStats());

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
        }*/
    }
}
