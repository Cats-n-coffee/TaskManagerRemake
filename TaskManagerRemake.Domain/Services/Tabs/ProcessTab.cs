using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.WPF.Services.Tabs
{
    public class ProcessTab
    {
        public ProcessTab()
        {
            InitProcessesTab();
        }
        public List<ProcessItem> InitProcessesTab()
        {
            Process[] allProcesses = Process.GetProcesses();
            List<ProcessItem> processList = new List<ProcessItem>();

            for (int i = 0; i < allProcesses.Length; i++)
            {
                Process process = allProcesses[i];
                ProcessItem processItem = new ProcessItem();

                processItem.Id = process.Id;
                processItem.Name = process.ProcessName;

                processList.Add(processItem);
            }

            return processList;
        }
    }
}
