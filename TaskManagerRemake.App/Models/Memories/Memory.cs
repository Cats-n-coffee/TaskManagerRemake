﻿namespace TaskManagerRemake.App.Models.Memories
{
    public class Memory
    {
        public int Total {  get; set; }
        public float InUse { get; set; }
        public float Available { get; set; }
        public float Committed { get; set; }
        public float Cached { get; set; }
        public float PagedPool { get; set; }
        public float NonPagedPool { get; set; }

        // Possibly Static values
        public int Speed {  get; set; }
        public int Slots { get; set; }
        public string FormFactor { get; set; }
        public string HardwareReserved { get; set; }
    }
}
