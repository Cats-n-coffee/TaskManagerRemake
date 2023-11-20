using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public class MemoryTab : IPerformanceItem
    {
        private readonly string title = "Memory";

        public MemoryTab()
        {
            InitPerformanceItem();
        }

        PerformanceCounter ramCounter;

        private void InitPerformanceItem()
        {
            this.ramCounter = new PerformanceCounter();

            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "Available MBytes";
        }

        public string GetTabTitle()
        {
            return title;
        }

        public string GetTabSpecs()
        {
            Int64 capacityInBytes = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory")
                .Get()
                .Cast<ManagementObject>()
                .Sum(x => Convert.ToInt64(x.Properties["Capacity"].Value));
            Int64 capacityInGb = capacityInBytes / Convert.ToInt64(Math.Pow(1024, 3));

            return capacityInGb.ToString();
        }

        public string GetAvailableRAM()
        {
            return this.ramCounter.NextValue() + "Mb";
        }
    }
}
