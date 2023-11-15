using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Services.Tabs;
using TaskManagerRemake.Domain.Models;
using System.Diagnostics;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class ProcessViewModel : BaseViewModel
    {
        private List<ProcessItem> _allProcesses;
        
        public List<ProcessItem> AllProcesses
        {
            get { return _allProcesses; }
            set
            {
                _allProcesses = value;
                OnPropertyChanged(nameof(_allProcesses));
            }
        }
        public ProcessViewModel()
        {
            Debug.WriteLine("constructor");
            
            _allProcesses = ProcessTab.GetProcessesForTab();

            foreach (var item in _allProcesses)
            {
                Debug.WriteLine($"name {item.Name} id {item.Id}");
            }
        }
    }
}
