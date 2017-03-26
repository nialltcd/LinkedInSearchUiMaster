using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkedInSearchUi.ViewModel
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private IModel _model;
        private List<Person> _allPeople;
        /// <summary>
        /// Initializes a new instance of the SearchViewModel class.
        /// </summary>
        public SearchViewModel(IModel model)
        {
            _model = model;
            //_allPeople = _model.ParseRawHtmlFilesFromDirectory();
            _allPeople = _model.GetPeople();
            //_model.GenerateCompanies(_allPeople);
            //var companyJobPairs = _model.GenerateCompanyJobPairs(_allPeople);
            //var companyJobPairs = _model.ParseCompanyJobPairsFromXml();
            
            //_model.CreateTrainingAndTestSetsNewRequirements(_allPeople);
            //_model.WriteCompanyJobPairsToXmlFile(companyJobPairs);
            //_model.WriteCompanyJobPairsTopStatisticsToXmlFile(companyJobPairs);
            //_model.WriteCompanyJobPairsTopStatisticsToFileFormatted(companyJobPairs);

            //_model.CreateTrainingAndTestSetsBasedOnCompany();
            //_model.CreateTrainingAndTestSetsBasedOnJob();
            SearchData = new ObservableCollection<Person>(_allPeople);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Person> _searchData;
        public ObservableCollection<Person> SearchData
        {
            get { return _searchData; }
            set
            {
                _searchData = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SearchButton { get { return new RelayCommand(SearchAction, CanSearch); } }

        private void SearchAction()
        {
            SearchData = new ObservableCollection<Person>(_model.LuceneSearch(SearchText));
        }
        private bool CanSearch()
        {
            return true;
            return !string.IsNullOrEmpty(_searchText);
        }

        public ICommand ResetButton { get { return new RelayCommand(ResetAction, CanReset); } }

        private void ResetAction()
        {
            SearchData = new ObservableCollection<Person>(_allPeople);
        }
        private bool CanReset()
        {
            return _allPeople.Count != _searchData.Count;
        }
    }
}
