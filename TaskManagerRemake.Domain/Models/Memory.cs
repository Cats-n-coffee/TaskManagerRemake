namespace TaskManagerRemake.Domain.Models
{
    public class Memory
    {
        public int Total { get; set; }
        public float InUse { get; set; }
        public float Available { get; set; }
        public float Committed { get; set; }
        public float Cached { get; set; }
        public float PagedPool { get; set; }
        public float NonPagedPool { get; set; }

        // Possibly Static values
    }
}
