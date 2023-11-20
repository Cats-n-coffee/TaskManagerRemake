using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public interface IPerformanceItem
    {
        string GetTabTitle();
        string GetTabSpecs();
    }
}
