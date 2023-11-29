using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public class CpuTab : IPerformanceItem
    {
        private readonly string title = "CPU";
        private CPUStaticData staticData = new CPUStaticData();

        PerformanceCounter cpuCounter;
        public CpuTab()
        {
            InitPerformanceItem();
        }

        // ================== Dynamic Stats ===============
        private void InitPerformanceItem()
        {
            this.cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
        }

        public float GetCurrentCpuUsage() // this might be part of a bigger function to use on interface
        {
            this.cpuCounter.NextValue();
            Thread.Sleep(1000);
            return this.cpuCounter.NextValue();
        }

        public int GetTotalProcesses()
        {
            Process[] allProcesses = Process.GetProcesses();
            int total = allProcesses.Length;

            Debug.WriteLine($"total {total}");
            return total;
        }

        public void GetTotalThreadsAndHandles()
        {
            Process[] allProcesses = Process.GetProcesses();
            int totalThreads = 0;
            int totalHandles = 0;

            for ( int i = 0; i < allProcesses.Length; i++ )
            {
                Process process = allProcesses[i];
                var allProcessThreads = process.Threads;
                totalThreads += allProcessThreads.Count;

                totalHandles += process.HandleCount;
            }

            Debug.WriteLine($"threads total: {totalThreads}");
            Debug.WriteLine($"handles total: {totalHandles}");
        }

        public void GetCpuCurrentSpeed()
        {
            PerformanceCounter cpu1Counter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
            double cpuValue = cpu1Counter.NextValue();
 
            Thread loop = new Thread(() => InfiniteLoop());
            loop.Start();

            Thread.Sleep(1000);
            cpuValue = cpu1Counter.NextValue();
            loop.Abort();

            foreach (ManagementObject obj in new ManagementObjectSearcher("SELECT *, Name FROM Win32_Processor").Get())
            {
                double maxSpeed = Convert.ToDouble(obj["MaxClockSpeed"]) / 1000;
                double turboSpeed = maxSpeed * cpuValue / 100;

                string res = string.Format("{0} Running at {1:0.00}Ghz, Turbo Speed: {2:0.00}Ghz", obj["Name"], maxSpeed, turboSpeed);
                Debug.WriteLine($"res {res}");
            }
        }

        public void GetSystemUpTime()
        {
            PerformanceCounter upTimeCounter = new PerformanceCounter("System", "System Up Time");
            upTimeCounter.NextValue();
   
            TimeSpan time = TimeSpan.FromSeconds(upTimeCounter.NextValue());
            string formattedTime = time.ToString("dd\\.hh\\:mm\\:ss");
        }
        private void InfiniteLoop()
        {
            int i = 0;

            while (true)
                i = i + 1 - 1;
        }

        // ============ Static Stats =============
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
                    staticData.LogicalProcessors = obj["NumberOfLogicalProcessors"].ToString();
                    staticData.L1Cache = GetL1Cache();
                    staticData.L2Cache = FormatCacheMemory(obj["L2CacheSize"]);
                    staticData.L3Cache = FormatCacheMemory(obj["L3CacheSize"]);
                    staticData.BaseSpeed = $"{obj["MaxClockSpeed"]} GHz";
                    staticData.Cores = obj["NumberOfCores"].ToString();
                    staticData.Sockets = obj["SocketDesignation"].ToString(); // this should probably be 1
                    staticData.IsVirtualizationEnabled = obj["VirtualizationFirmwareEnabled"].ToString();

                    spec = obj["Name"].ToString();
                }
            }

            return spec;
        }

        private string GetL1Cache()
        {
            double L1CacheResult = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_CacheMemory");
            
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                if ((ushort)obj?.GetPropertyValue("Level") == (ushort)3)
                {
                    L1CacheResult += Convert.ToInt32(obj?.GetPropertyValue("MaxCacheSize"));
                }
            }

            if (L1CacheResult > 0) Math.Round(L1CacheResult /= 1024, 2);

            return $"{L1CacheResult.ToString("0.0")} MB";
        }

        public string FormatCacheMemory(object cacheSize)
        {
            int size = Convert.ToInt32(cacheSize);

            if (size <= 0) return $"0 MB";
            
            return $"{(size / 1024.0f).ToString("0.0")} MB";
        }

        public List<StaticPerformanceStats> GetStaticStats()
        {
            GetTabSpecs();
            
            List<StaticPerformanceStats> staticStatsList = new List<StaticPerformanceStats>();
            var staticDataToList = staticData.GetType().GetProperties().ToList();
            
            foreach (System.Reflection.PropertyInfo stat in staticDataToList)
            {
                StaticPerformanceStats staticItem = new StaticPerformanceStats();
                staticItem.StaticPerformanceKey = stat.Name;
                staticItem.StaticPerformanceValue = stat.GetValue(staticData, null).ToString();

                staticStatsList.Add(staticItem);
            }

            return staticStatsList;
        }
    }
}
