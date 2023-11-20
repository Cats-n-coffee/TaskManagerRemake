using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerRemake.Domain.Models
{
    public class CPUStaticData
    {
        public string BaseSpeed { get; set; } = "0 GHz";
        public string Sockets { get; set; } = "0";
        public string Cores { get; set; } = "0";
        public string LogicalProcessors { get; set; } = "0";
        public string L1Cache { get; set; } = "0 MB";
        public string L2Cache { get; set; } = "0 MB";
        public string L3Cache { get; set; } = "0 MB";
        public string IsVirtualizationEnabled { get; set; } = "Enabled";
    }
}
