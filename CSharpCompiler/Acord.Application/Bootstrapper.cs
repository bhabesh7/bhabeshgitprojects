using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acord.Application
{
    public class Bootstrapper:UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            this.Shell
        }

        protected override void InitializeShell()
        {
            Application.App.Current.MainWindow.Show();
            //Application.Current.MainWindow.Show();
        }
    }
}
