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
    public class StatisticsViewModel : ViewModelBase
    {
        private GraphingService _graphingService;
        public StatisticsViewModel()
        {
            _graphingService = new GraphingService();
            TopCompanyJobPairsPlot = _graphingService.GenerateTopCompanyJobPairs();
        }

        public string Title { get; private set; }

        private IList<DataPoint> _points;
        public IList<DataPoint> Points
        {
            get { return _points; }
            private set
            {
                _points = value;
                RaisePropertyChanged();
            }
        }

        public PlotModel TopCompanyJobPairsPlot
        {
            get;set;
        }
    }
}
