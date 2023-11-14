namespace TaskManagerRemake.Domain.Models
{
    public class ProcessItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UsageCPU { get; set; } = 0;
        public int UsageMemory { get; set; } = 0;
        public int UsageDisk { get; set; } = 0;
        public int UsageNetwork { get; set; } = 0;
    }
}
