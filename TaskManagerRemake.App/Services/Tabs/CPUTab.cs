using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// http://zamov.online.fr/EXHTML/CSharp/CSharp_927308.html
namespace TaskManagerRemake.App.Services.Tabs
{
    public class CPUTab
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        public CPUTab()
        { 
            InitCPUTab();
        }
        public void InitCPUTab()
        {
            this.cpuCounter = new PerformanceCounter();
            this.ramCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "Available MBytes";
        }

        public string getCurrentCpuUsage()
        {
            return this.cpuCounter.NextValue() + "%";
        }

        public string getAvailableRAM()
        {
            return this.ramCounter.NextValue() + "Mb";
        }
    }
}
