using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Graphing
{
    public interface IGraphingService
    {
        PlotModel GenerateTopCompanyJobPairs();
        PlotModel GenerateUsefulProfilePieChart();
    }
}
