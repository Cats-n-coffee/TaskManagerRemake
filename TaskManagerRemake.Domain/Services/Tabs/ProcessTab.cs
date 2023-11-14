using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerRemake.WPF.Services.Tabs
{
    public class ProcessTab
    {
        public ProcessTab()
        {
            InitProcessesTab();
        }
        public void InitProcessesTab()
        {
            Process[] allProcesses = Process.GetProcesses();

            for (int i = 0; i < allProcesses.Length; i++)
            {
                Process process = allProcesses[i];
            }
        }
    }
}
