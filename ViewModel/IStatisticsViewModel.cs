using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.ViewModel
{
    public interface IStatisticsViewModel
    {
        PlotModel TopCompanyJobPairsPlot { get; }
        PlotModel ProfileUsefulnessPlot { get; }
    }
}
