using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.Domain.Services.Tabs
{
    public static class ProcessTab
    {
        public static List<ProcessItem> GetProcessesForTab()
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
