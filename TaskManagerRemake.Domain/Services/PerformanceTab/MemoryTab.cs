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
        PerformanceCounter ramCounter;

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
            this.ramCounter = new PerformanceCounter();

            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "Available MBytes";

            GetRAMCapacity();
        }

        public int GetDataForChart()
        {
            return 0;
        }

        private int GetInUseCompressedMemory()
        {
            return 0;
        }

        private double GetAvailableMemory()
        {
            var val = (this.ramCounter.NextValue() / 1000);

            return Math.Round((double)val, 1);
        }

        private int GetCommitedMemory()
        {  return 0;}

        private int GetCachedMemory()
        {
            return 0;
        }

        private int GetPagedMemoryPool()
        {
            return 0;
        }

        private int GetNonPagedMemoryPool()
        {
            return 0;
        }

        public List<PerformanceStat> GetDynamicStats()
        {
            Debug.WriteLine(GetAvailableMemory());
            List<PerformanceStat> stats = new List<PerformanceStat>();
            return stats;
        }

        // ======================= Static Stats =========================
        private int GetRAMCapacity()
        {
            Int64 capacityInBytes = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory")
                .Get()
                .Cast<ManagementObject>()
                .Sum(x => Convert.ToInt64(x.Properties["Capacity"].Value));
            Int64 capacityInGb = capacityInBytes / Convert.ToInt64(Math.Pow(1024, 3));

            totalPhysicalMemory = (ulong)capacityInBytes / Convert.ToUInt64(1024); // kb

            return (int)capacityInGb;
        }

        public string GetTotalMemorySlots()
        {
            string res;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_PhysicalMemoryArray"))
            {
                ManagementObjectCollection objColl = searcher.Get();
                ManagementObject obj = objColl.OfType<ManagementObject>().FirstOrDefault();
                res = obj["MemoryDevices"] != null ? obj["MemoryDevices"].ToString() : string.Empty;
                Debug.WriteLine($"Res is {res}");
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
