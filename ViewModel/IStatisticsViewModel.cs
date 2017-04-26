using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkedInSearchUi.ViewModel
{
    public interface IStatisticsViewModel
    {
        PlotModel TopCompanyJobPairsPlot { get; }
        PlotModel TopJobStatsPlot { get; }
        PlotModel TopCompanyStatsPlot { get; }
        PlotModel ProfileUsefulnessPlot { get; }
        PlotModel MachineLearningAccuracyPlot { get; }
        int RandomForestSize { get; }
        int SkillSetSize { get; }
        ICommand UpdatePerformanceStatistics { get; }
    }
}
