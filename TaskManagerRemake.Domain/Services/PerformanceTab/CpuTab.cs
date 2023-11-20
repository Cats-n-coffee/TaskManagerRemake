using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public class CpuTab : IPerformanceItem
    {
        private readonly string title = "CPU";

        PerformanceCounter cpuCounter;
        public CpuTab()
        {
            InitPerformanceItem();
        }

        private void InitPerformanceItem()
        {
            this.cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
        }

        public string GetTabTitle()
        {
            return title;
        }

        public string GetTabSpecs()
        {
            string spec = string.Empty;
            using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"))
            {
                foreach (ManagementObject obj in win32Proc.Get())
                {
                    spec = obj["Name"].ToString();
                }
            }

            return spec;
        }

        public string GetCurrentCpuUsage() // this might be part of a bigger function to use on interface
        {
            return this.cpuCounter.NextValue() + "%";
        }
    }
}
