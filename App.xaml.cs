using LinkedInSearchUi.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using System.Reflection;
using LinkedInSearchUi.ViewModel;
using LinkedInSearchUi.Resources;

namespace LinkedInSearchUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IocContainer.Initialise(new DiLinkedIn());
            var windowFactory = IocContainer.Get<IWindowFactory>();
            var mainWindow = windowFactory.Create();
            mainWindow.Show();
                                  
            base.OnStartup(e);
        }
    }
}
