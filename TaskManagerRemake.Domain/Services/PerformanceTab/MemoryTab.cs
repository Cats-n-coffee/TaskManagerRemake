using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public class MemoryTab : IPerformanceItem
    {
        private readonly string title = "Memory";
        private readonly MemoryStaticData MemoryStaticData = new MemoryStaticData();
        private ulong totalPhysicalMemory = 0;
        private int ramCapacity = 0;

        PerformanceCounter availableRamCounter;
        // Committed
        PerformanceCounter committedBytesCounter;
        PerformanceCounter commitLimitCounter;
        // Cached
        PerformanceCounter cachedMemoryCounter;
        PerformanceCounter modifiedPageListBytesCounter;
        PerformanceCounter standbyCacheCoreBytesCounter;
        PerformanceCounter standbyCacheNormalPriorityBytesCounter;
        PerformanceCounter standbyCacheReserveBytesCounter;
        // Virtual
        PerformanceCounter pagedPoolCounter;
        PerformanceCounter nonPagedPoolCounter;

        public MemoryTab()
        {
            InitPerformanceItem();
        }

        // =========================== Tab Static Texts ======================
        public string GetTabTitle()
        {
            return title;
        }

        public string GetTabSpecs()
        {
            return $"{ramCapacity} GB";
        }

        // ========================= Dynamic Stats ========================
        private void InitPerformanceItem()
        {
            this.availableRamCounter = new PerformanceCounter("Memory", "Available MBytes");

            GetRAMCapacity();

            // Committed
            this.committedBytesCounter = new PerformanceCounter("Memory", "Committed Bytes");
            this.commitLimitCounter = new PerformanceCounter("Memory", "Commit Limit");

            // Cached
            this.cachedMemoryCounter = new PerformanceCounter("Memory", "Cache Bytes");
            this.modifiedPageListBytesCounter = new PerformanceCounter("Memory", "Modified Page List Bytes");
            this.standbyCacheCoreBytesCounter = new PerformanceCounter("Memory", "Standby Cache Core Bytes");
            this.standbyCacheNormalPriorityBytesCounter = new PerformanceCounter("Memory", "Standby Cache Normal Priority Bytes");
            this.standbyCacheReserveBytesCounter = new PerformanceCounter("Memory", "Standby Cache Reserve Bytes");

            // Virtual
            this.pagedPoolCounter = new PerformanceCounter("Memory", "Pool Paged Bytes");
            this.nonPagedPoolCounter = new PerformanceCounter("Memory", "Pool Nonpaged Bytes");
        }

        public int GetDataForChart()
        {
            return 0;
        }

        private double GetInUseMemory()
        {
            double val = Math.Round(this.availableRamCounter.NextValue() / 1000, 1);
            double inUseMemory = (double)ramCapacity - val;
            // This number seems incorrect, we might need to subtract something
            return inUseMemory;
        }

        private double GetAvailableMemory()
        {
            double val = (this.availableRamCounter.NextValue() / 1000);
            return Math.Round(val, 1);
        }

        private double GetCommittedBytes()
        {
            double committedBytes = this.committedBytesCounter.NextValue() / Math.Pow(1024, 3);
            return Math.Round((double)committedBytes, 1);
        }

        private double GetCommitLimit()
        {
            double commitLimit = this.commitLimitCounter.NextValue() / Math.Pow(1024, 3);
            return Math.Round((double)commitLimit, 1);
        }

        private double GetCachedMemory()
        {
            double cachedBytes = this.cachedMemoryCounter.NextValue();
            double modifiedPageListBytes = this.modifiedPageListBytesCounter.NextValue();
            double standbyCacheCoreBytes = this.standbyCacheCoreBytesCounter.NextValue();
            double standbyCachedNormalPriorityBytes = this.standbyCacheNormalPriorityBytesCounter.NextValue();
            double standbyCachedReserveBytes = this.standbyCacheReserveBytesCounter.NextValue();

            double totalCachedInGb = (cachedBytes + modifiedPageListBytes + standbyCacheCoreBytes
                + standbyCachedNormalPriorityBytes + standbyCachedReserveBytes) / Math.Pow(1024, 3);
      
            return Math.Round(totalCachedInGb, 1);
        }

        private double GetPagedMemoryPool()
        {
            double val = this.pagedPoolCounter.NextValue() / Math.Pow(1024, 3);
            return Math.Round(val, 1);
        }

        private double GetNonPagedMemoryPool()
        {
            double val = this.nonPagedPoolCounter.NextValue() / Math.Pow(1024, 3);
            return Math.Round(val, 1);
        }

        public List<PerformanceStat> GetDynamicStats()
        {
            Debug.WriteLine("In Use " + GetInUseMemory());
            Debug.WriteLine("Available " + GetAvailableMemory());
            Debug.WriteLine($"cached {GetCachedMemory()}");
            Debug.WriteLine($"committed bytes {GetCommittedBytes()}");
            Debug.WriteLine($"commit limit {GetCommitLimit()}");
            Debug.WriteLine("Paged " + GetPagedMemoryPool());
            Debug.WriteLine("Non paged " + GetNonPagedMemoryPool());
            List<PerformanceStat> stats = new List<PerformanceStat>();
            return stats;
        }

        // ======================= Static Stats =========================
        private void GetRAMCapacity()
        {
            Int64 capacityInBytes = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory")
                .Get()
                .Cast<ManagementObject>()
                .Sum(x => Convert.ToInt64(x.Properties["Capacity"].Value));
            Int64 capacityInGb = capacityInBytes / Convert.ToInt64(Math.Pow(1024, 3));

            totalPhysicalMemory = (ulong)capacityInBytes / Convert.ToUInt64(1024); // kb
            ramCapacity = (int)capacityInGb;
        }

        public string GetTotalMemorySlots()
        {
            string res;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_PhysicalMemoryArray"))
            {
                ManagementObjectCollection objColl = searcher.Get();
                ManagementObject obj = objColl.OfType<ManagementObject>().FirstOrDefault();
                res = obj["MemoryDevices"] != null ? obj["MemoryDevices"].ToString() : string.Empty;
            }

            return res;
        }

        // Keeping values in Kb for subtraction to maintain accuracy of Task Manager
        private string GetHardwareReservedMemory()
        {
            ulong totalVisibleMemory = 0;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    totalVisibleMemory = (ulong)obj["TotalVisibleMemorySize"]; // kb
                }
            }

            ulong hardwareReserved = (totalPhysicalMemory - totalVisibleMemory) / 1024;

            return $"{hardwareReserved} MB";
        }

        public void GetMemoryInfo()
        {
            Debug.WriteLine("get speed top");
            int slotsUsed = 0;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_PhysicalMemory"))
            {
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    if (obj["Speed"] != null)
                    {
                        Debug.WriteLine("Speed obj");
                        MemoryStaticData.Speed = $"{obj["Speed"]} MHz";
                        MemoryStaticData.FormFactor = obj["FormFactor"].ToString(); // this returns an int, is there an enum?
                        MemoryStaticData.HardwareReserved = GetHardwareReservedMemory();

                        if (obj["DeviceLocator"].ToString() != string.Empty) slotsUsed++;
                        MemoryStaticData.Slots = $"{slotsUsed} of {GetTotalMemorySlots()}";
                    }
                }
            }
        }

        public List<PerformanceStat> GetStaticStats()
        {
            GetMemoryInfo();

            List<PerformanceStat> staticStats = new List<PerformanceStat>();
            var staticDataToList = MemoryStaticData.GetType().GetProperties().ToList();

            foreach ( var item in staticDataToList )
            {
                PerformanceStat stat = new PerformanceStat();
                stat.PerformanceStatKey = item.Name;
                stat.PerformanceStatValue = item.GetValue(MemoryStaticData, null).ToString();

                staticStats.Add(stat);
            }
 
            return staticStats;
        }
    }
}
