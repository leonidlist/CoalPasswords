using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;

namespace CoalPasswords
{
    class MyModule:NinjectModule
    {
        public override void Load()
        {
            this.Bind<LoginWindowViewModel>().To<LoginWindowViewModel>();
        }
    }
}
