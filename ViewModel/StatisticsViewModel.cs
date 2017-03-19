using GalaSoft.MvvmLight;
using LinkedInSearchUi.Graphing;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.ViewModel
{
    public class StatisticsViewModel : ViewModelBase, IStatisticsViewModel
    {
        private IGraphingService _graphingService;
        public StatisticsViewModel(IGraphingService graphingService)
        {
            _graphingService = graphingService;
            TopCompanyJobPairsPlot = _graphingService.GenerateTopCompanyJobPairs();
            ProfileUsefulnessPlot = _graphingService.GenerateUsefulProfilePieChart();
        }

        private PlotModel _topCompanyJobPairs;
        public PlotModel TopCompanyJobPairsPlot
        {
            get { return _topCompanyJobPairs; }
            set {
                _topCompanyJobPairs = value;
                RaisePropertyChanged();
            }
        }
        private PlotModel _profileUsefulnessPlot;
        public PlotModel ProfileUsefulnessPlot
        {
            get { return _profileUsefulnessPlot; }
            set
            {
                _profileUsefulnessPlot = value;
                RaisePropertyChanged();
            }
        }
    }
}
