using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpCompiler
{
    public class Bootstrapper:UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return null;
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
