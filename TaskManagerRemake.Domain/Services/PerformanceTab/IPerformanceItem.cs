using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.Domain.Models;

namespace TaskManagerRemake.Domain.Services.PerformanceTab
{
    public interface IPerformanceItem
    {
        string GetTabTitle();
        string GetTabSpecs();
        int GetDataForChart();
        List<PerformanceStat> GetDynamicStats();
        List<PerformanceStat> GetStaticStats();
        string GetThumbnailData();
    }
}
