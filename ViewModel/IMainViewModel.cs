using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkedInSearchUi.ViewModel
{
    public interface IMainViewModel
    {
        ViewModelBase CurrentView { get; set; }
        ICommand OpenSearchConsole { get; }
        ICommand OpenStatisticsConsole { get; }
    }
}
