using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Resources
{
    public static class IocContainer
    {
        private static IKernel _kernel;
        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public static void Initialise(params INinjectModule[] modules)
        {
            if(_kernel == null)
            {
                _kernel = new StandardKernel(modules);
            }
        }
    }
}
