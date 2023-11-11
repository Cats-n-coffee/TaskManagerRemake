using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// http://zamov.online.fr/EXHTML/CSharp/CSharp_927308.html
namespace TaskManagerRemake.App.Services.Tabs
{
    public class PerformanceTab
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        public PerformanceTab()
        { 
            InitPerformanceTab();
        }
        public void InitPerformanceTab()
        {
            this.cpuCounter = new PerformanceCounter();
            this.ramCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "Available MBytes";
        }

        public string GetCurrentCpuUsage()
        {
            return this.cpuCounter.NextValue() + "%";
        }

        public string GetAvailableRAM()
        {
            return this.ramCounter.NextValue() + "Mb";
        }
    }
}
