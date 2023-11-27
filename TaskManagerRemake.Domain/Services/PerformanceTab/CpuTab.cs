using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string GetCurrentCpuUsage() // this might be part of a bigger function to use on interface
        {
            return this.cpuCounter.NextValue() + "%";
        }
    }
}
