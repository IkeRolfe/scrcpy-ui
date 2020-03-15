using Caliburn.Micro;
using scrcpy_ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace scrcpy_ui
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
