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
                    // staticData.L1Cache = obj[].ToString(); need another API
                    staticData.L2Cache = $"{obj["L2CacheSize"]} MB";
                    staticData.L3Cache = $"{obj["L3CacheSize"]} MB";
                    staticData.BaseSpeed = $"{obj["MaxClockSpeed"]} GHz";
                    staticData.Cores = obj["NumberOfCores"].ToString();
                    staticData.Sockets = obj["SocketDesignation"].ToString(); // this should probably be 1
                    staticData.IsVirtualizationEnabled = obj["VirtualizationFirmwareEnabled"].ToString();

                    spec = obj["Name"].ToString();
                }
            }

            return spec;
        }

        public List<StaticPerformanceStats> GetStaticStats()
        {

            return new List<StaticPerformanceStats>();
        }

        public string GetCurrentCpuUsage() // this might be part of a bigger function to use on interface
        {
            return this.cpuCounter.NextValue() + "%";
        }
    }
}
