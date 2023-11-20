using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public string GetAvailableRAM()
        {
            return this.ramCounter.NextValue() + "Mb";
        }
    }
}
