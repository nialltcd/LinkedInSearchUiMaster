using GalaSoft.MvvmLight;
using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkedInSearchUi.ViewModel
{
    public interface ISearchViewModel
    {
        string SearchText{ get; set;}
        ObservableCollection<Person> SearchData { get; set; }
        ICommand SearchButton { get; }
        ICommand ResetButton{ get; }
    }
}
