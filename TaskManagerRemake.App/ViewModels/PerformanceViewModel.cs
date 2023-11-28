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
        private readonly CpuTab _cpuTab;
        private readonly MemoryTab _memoryTab;

        private string _cpuUsage;

        public string CpuUsage
        {
            get => _cpuUsage;
            set {
                _cpuUsage = value;
                OnPropertyChanged(nameof(CpuUsage));
            }
        }

        private string _ramUsage;
        public string RamUsage
        {
            get => _ramUsage;
            set
            {
                _ramUsage = value;
                OnPropertyChanged(nameof(RamUsage));
            }
        }

        public ObservableCollection<StaticPerformanceStats> StaticStats { get; set; }

        public List<IPerformanceItem> performanceDetailsList = new List<IPerformanceItem> ();


        public PerformanceViewModel()
        {
            _cpuTab = new CpuTab();
            _memoryTab = new MemoryTab();

            InitCpuTimer();

            RamUsage = _memoryTab.GetAvailableRAM();

            StaticStats = new ObservableCollection<StaticPerformanceStats>(_cpuTab.GetStaticStats());
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
            float currentUsage = _cpuTab.GetCurrentCpuUsage();           
            CpuUsage = $"{Math.Round(currentUsage)} %";

            CommandManager.InvalidateRequerySuggested();
        }
    }
}
