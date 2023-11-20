using System;

namespace TaskManagerRemake.Domain.Models
{
    public class CPUDynamicData
    {
        public string Name { get; set; }
        public int Utilization { get; set; }
        public float Speed { get; set; }
        public int Processes { get; set; }
        public int Threads { get; set; }
        public int Handles { get; set; }
        public TimeSpan UpTime { get; set; }
    }
}
