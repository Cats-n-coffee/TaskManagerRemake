using System;

namespace TaskManagerRemake.WPF.Models.CPUs
{
    public class CPU
    {
        public string Name { get; set; }
        public int Utilization { get; set; }
        public float Speed { get; set; }
        public int Processes { get; set; }
        public int Threads { get; set; }
        public int Handles { get; set; }
        public TimeSpan UpTime { get; set; }

        // The following seem to be static values
        public float BaseSpeed { get; set; }
        public int Sockets { get; set; }
        public int Cores { get; set; }
        public int LogicalProcessors {  get; set; }
        public float L1Cache {  get; set; }
        public float L2Cache { get; set;}
        public float L3Cache { get; set;}
    }
}
