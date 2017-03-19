using LinkedInSearchUi.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Resources
{
    public class WindowFactory : IWindowFactory
    {
        private IKernel _kernel;
        public WindowFactory(IKernel kernel)
        {
            _kernel = kernel;
        }
        public MainWindow Create()
        {
            var window = new MainWindow();
            var viewModel = _kernel.Get<IMainViewModel>();
            window.DataContext = viewModel;
            return window;
        }
    }
}
