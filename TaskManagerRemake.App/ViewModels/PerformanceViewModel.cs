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


namespace TaskManagerRemake.WPF.ViewModels
{
    public class PerformanceViewModel : ViewModelBase
    {
        public ObservableCollection<PerformanceItemDisplay> PerformanceItems { get; set; }
 
        public PerformanceViewModel()
        {
            PerformanceItems = new ObservableCollection<PerformanceItemDisplay>(
                new List<PerformanceItemDisplay>() 
                {
                    new PerformanceItemDisplay("cpu"),
                    new PerformanceItemDisplay("memory"),
                }
                );
        }
    }
}
