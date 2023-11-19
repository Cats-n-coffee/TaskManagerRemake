using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Services.Tabs;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceViewModel : ViewModelBase
    {
        private readonly PerformanceTab _performanceTab;
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


        public PerformanceViewModel()
        {
            _performanceTab = new PerformanceTab();

            CpuUsage = _performanceTab.GetCurrentCpuUsage();
            RamUsage = _performanceTab.GetAvailableRAM();
        }

    }
}
