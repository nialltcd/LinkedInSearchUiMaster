using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.ViewModel
{
    public interface IStatisticsViewModel
    {
        PlotModel TopCompanyJobPairsPlot { get; }
        PlotModel TopJobStatsPlot { get; }
        PlotModel TopCompanyStatsPlot { get; }
        PlotModel ProfileUsefulnessPlot { get; }
    }
}
