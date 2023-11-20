using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Services.PerformanceTab;

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
                OnPropertyChanged(nameof(_cpuUsage));
            }
        }

        private string _ramUsage;
        public string RamUsage
        {
            get => _ramUsage;
            set
            {
                _ramUsage = value;
                OnPropertyChanged(nameof(_ramUsage));
            }
        }

        public List<IPerformanceItem> performanceDetailsList = new List<IPerformanceItem> ();


        public PerformanceViewModel()
        {
            _cpuTab = new CpuTab();
            _memoryTab = new MemoryTab();

            CpuUsage = _cpuTab.GetCurrentCpuUsage();
            RamUsage = _memoryTab.GetAvailableRAM();
        }

    }
}
