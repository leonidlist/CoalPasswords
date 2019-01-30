using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace CoalPasswords
{
    class ViewModelLocator
    {
        private IKernel _kernel;
        public LoginWindowViewModel LoginViewModel { get => _kernel.Get<LoginWindowViewModel>(); }
        public ViewModelLocator()
        {
            _kernel = new StandardKernel(new MyModule());
        }
    }
}
