using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;
using TaskManagerRemake.Domain.Services.PerformanceTab;
using System.Windows.Threading;
using System.Windows.Input;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceViewModel : ViewModelBase
    {
        private readonly IPerformanceItem performanceItem;

        public ObservableCollection<PerformanceStat> StaticStats { get; set; }
        private ObservableCollection<PerformanceStat> _dynamicStats;
        public ObservableCollection<PerformanceStat> DynamicStats
        {
            get => _dynamicStats;
            set
            {
                _dynamicStats = value;
                OnPropertyChanged(nameof(DynamicStats));
            }
        }

        public List<IPerformanceItem> performanceDetailsList = new List<IPerformanceItem> ();


        public PerformanceViewModel()
        {
            // init the Performance tab with the CPU tab as default
            // any tab that is clicked after that will update the performanceItem field
            performanceItem = new CpuTab();

            InitCpuTimer();

            StaticStats = new ObservableCollection<PerformanceStat>(performanceItem.GetStaticStats());
            DynamicStats = new ObservableCollection<PerformanceStat>(performanceItem.GetDynamicStats());
        }

        private void InitCpuTimer()
        {
            DispatcherTimer cpuUsageTimer = new DispatcherTimer();

            cpuUsageTimer.Tick += new EventHandler(CpuUsageTimer_Tick);
            cpuUsageTimer.Interval = TimeSpan.FromSeconds(1);
            cpuUsageTimer.Start();
        }

        private void CpuUsageTimer_Tick(Object source, EventArgs e)
        {
            DynamicStats = new ObservableCollection<PerformanceStat>(performanceItem.GetDynamicStats());

            CommandManager.InvalidateRequerySuggested();
        }
    }
}
