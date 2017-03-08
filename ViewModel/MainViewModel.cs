using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Model;
using System.Collections.Generic;

namespace LinkedInSearchUi.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Model.Model _model;
        private SearchViewModel _searchViewModel;
        private StatisticsViewModel _statisticsViewModel;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _model = new Model.Model();
            _searchViewModel = new SearchViewModel();
            _statisticsViewModel = new StatisticsViewModel();
            CurrentView = _searchViewModel;
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                RaisePropertyChanged();
            }
        }
       
        public ICommand OpenSearchConsole {  get { return new RelayCommand(OpenSearchConsoleAction, CanOpenSearchConsole); } }

        private void OpenSearchConsoleAction()
        {
            CurrentView = _searchViewModel;
        }
        private bool CanOpenSearchConsole()
        {
            return true;
        }

        public ICommand OpenStatisticsConsole { get { return new RelayCommand(OpenStatisticsConsoleAction, CanOpenStatisticsConsole); } }

        private void OpenStatisticsConsoleAction()
        {
            CurrentView = _statisticsViewModel;
        }
        private bool CanOpenStatisticsConsole()
        {
            return true;
        }

    }
}