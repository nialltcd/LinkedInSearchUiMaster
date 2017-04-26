using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LinkedInSearchUi.Graphing;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LinkedInSearchUi.ViewModel
{
    public class StatisticsViewModel : ViewModelBase, IStatisticsViewModel
    {
        private IGraphingService _graphingService;
        public StatisticsViewModel(IGraphingService graphingService)
        {
            _graphingService = graphingService;
            TopCompanyJobPairsPlot = _graphingService.GenerateTopCompanyJobPairs();
            TopJobStatsPlot = _graphingService.GenerateTopJobStats();
            TopCompanyStatsPlot = _graphingService.GenerateTopCompanyStats();
            TopSkillStatsPlot = _graphingService.GenerateTopSkillStats();
            ProfileUsefulnessPlot = _graphingService.GenerateUsefulProfilePieChart();
            MachineLearningAccuracyPlot = _graphingService.GenerateMachineLearningAccuracy();
            RandomForestSize = _graphingService.GenerateRandomForestSize();
            SkillSetSize = _graphingService.GenerateSkillSetSize();
            UpdatingPerformanceStatisticsInProgressVisibility = Visibility.Hidden;
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
        private PlotModel _topJobStatsPairs;
        public PlotModel TopJobStatsPlot
        {
            get { return _topJobStatsPairs; }
            set
            {
                _topJobStatsPairs = value;
                RaisePropertyChanged();
            }
        }
        private PlotModel _topCompanyStats;
        public PlotModel TopCompanyStatsPlot
        {
            get { return _topCompanyStats; }
            set
            {
                _topCompanyStats = value;
                RaisePropertyChanged();
            }
        }
        private PlotModel _topSkillStats;
        public PlotModel TopSkillStatsPlot
        {
            get { return _topSkillStats; }
            set
            {
                _topSkillStats = value;
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

        private PlotModel _machineLearningAccuracyPlot;
        public PlotModel MachineLearningAccuracyPlot
        {
            get { return _machineLearningAccuracyPlot; }
            set
            {
                _machineLearningAccuracyPlot = value;
                RaisePropertyChanged();
            }
        }

        private int _randomForestSize;
        public int RandomForestSize {
            get { return _randomForestSize; }
            set { _randomForestSize = value;
                RaisePropertyChanged();
            }
        }

        private int _skillSetSize;
        public int SkillSetSize
        {
            get { return _skillSetSize; }
            set { _skillSetSize = value;
                RaisePropertyChanged();
            }
        }

        private string _jobTitle;
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                _jobTitle = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _updatingPerformanceStatisticsInProgressVisibility;
        public Visibility UpdatingPerformanceStatisticsInProgressVisibility
        {
            get { return _updatingPerformanceStatisticsInProgressVisibility; }
            set { _updatingPerformanceStatisticsInProgressVisibility = value;
                RaisePropertyChanged();
            }
        }

        public ICommand UpdatePerformanceStatistics { get { return new RelayCommand(UpdatePerformanceStatisticsAction, () => true); } }

        private void UpdatePerformanceStatisticsAction()
        {    
            new Task(() => {
                UpdatingPerformanceStatisticsInProgressVisibility = Visibility.Visible;
                _graphingService.UpdatePerformanceStatisticsAction(RandomForestSize, SkillSetSize, JobTitle);
                MachineLearningAccuracyPlot = _graphingService.GenerateMachineLearningAccuracy();
                UpdatingPerformanceStatisticsInProgressVisibility = Visibility.Hidden;
            }).Start();
        }
    }
}
