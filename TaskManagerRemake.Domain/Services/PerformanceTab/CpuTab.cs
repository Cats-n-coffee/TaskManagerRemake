﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Management;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public class CpuTab : IPerformanceItem
    {
        private readonly string title = "CPU";
        private string Spec { get; set; } = string.Empty;
        private CPUStaticData staticData = new CPUStaticData();
        private int cpuUsage = 0;
        private double cpuMaxSpeed = 0;

        PerformanceCounter cpuTimeCounter;
        PerformanceCounter cpuPerformanceCounter;
        public CpuTab()
        {
            InitPerformanceItem();
        }

        // ================== Tab Static Texts ===============
        public string GetTabTitle()
        {
            return title;
        }

        public string GetTabSpecs()
        {
            return Spec;
        }

        // ================== Dynamic Stats ===============
        private void InitPerformanceItem()
        {
            this.cpuTimeCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            this.cpuPerformanceCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
        }

        private void GetCurrentCpuUsage()
        {
            this.cpuTimeCounter.NextValue();
            Thread.Sleep(750);
            cpuUsage = (int)Math.Round(this.cpuTimeCounter.NextValue());
        }

        public string GetTotalProcesses()
        {
            Process[] allProcesses = Process.GetProcesses();
            int total = allProcesses.Length;

            return total.ToString();
        }

        public string[] GetTotalThreadsAndHandles()
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

            string[] allTotals = new string[2];
            allTotals[0] = totalThreads.ToString();
            allTotals[1] = totalHandles.ToString();

            return allTotals;
        }

        public string GetCpuCurrentSpeed()
        {
            cpuPerformanceCounter.NextValue();
            Thread.Sleep(1000);
            double cpuPerf = cpuPerformanceCounter.NextValue();

            double maxSpeed = cpuMaxSpeed / 1000;
            double turboSpeed = maxSpeed * cpuPerf / 100;

            string currentSpeed = String.Format("{0:0.00} GHz", turboSpeed);

            return currentSpeed;
        }

        public string GetSystemUpTime()
        {
            PerformanceCounter upTimeCounter = new PerformanceCounter("System", "System Up Time");
            upTimeCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(upTimeCounter.NextValue());

            return time.ToString("dd\\.hh\\:mm\\:ss");
        }

        public int GetDataForChart()
        {
            return cpuUsage;
        }

        public List<PerformanceStat> GetDynamicStats()
        {
            GetCurrentCpuUsage();

            string[] threadsAndHandles = GetTotalThreadsAndHandles();
            List<PerformanceStat> stats = new List<PerformanceStat>
            {
                new PerformanceStat()
                {
                    PerformanceStatKey = "Utilization",
                    PerformanceStatValue = $"{cpuUsage}%"
                },
                new PerformanceStat()
                {
                    PerformanceStatKey = "Speed",
                    PerformanceStatValue = GetCpuCurrentSpeed()
                },
                new PerformanceStat()
                {
                    PerformanceStatKey = "Processes",
                    PerformanceStatValue = GetTotalProcesses()
                },
                new PerformanceStat()
                {
                    PerformanceStatKey = "Threads",
                    PerformanceStatValue = threadsAndHandles[0]
                },
                new PerformanceStat()
                {
                    PerformanceStatKey = "Handles",
                    PerformanceStatValue = threadsAndHandles[1]
                },
                new PerformanceStat()
                {
                    PerformanceStatKey = "Up Time",
                    PerformanceStatValue = GetSystemUpTime()
                }
            };

            return stats;
        }

        // ============ Static Stats =============

        public void GetProcessorInfo()
        {
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
                    staticData.Sockets = "1";
                    staticData.IsVirtualizationEnabled = obj["VirtualizationFirmwareEnabled"].ToString();

                    Spec = obj["Name"].ToString();
                    cpuMaxSpeed = Convert.ToDouble(obj["MaxClockSpeed"]);
                }
            }
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

        public List<PerformanceStat> GetStaticStats()
        {
            GetProcessorInfo();
            
            List<PerformanceStat> staticStatsList = new List<PerformanceStat>();
            var staticDataToList = staticData.GetType().GetProperties().ToList(); // Array of keys
            
            foreach (System.Reflection.PropertyInfo stat in staticDataToList)
            {
                PerformanceStat staticItem = new PerformanceStat();
                staticItem.PerformanceStatKey = stat.Name;
                staticItem.PerformanceStatValue = stat.GetValue(staticData, null).ToString();

                staticStatsList.Add(staticItem);
            }

            return staticStatsList;
        }

        // ================================ Thumbnail =============================
        public string GetThumbnailData()
        {
            return $"{cpuUsage}%";
        }
    }
}
