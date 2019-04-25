using Accord.Interfaces;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Accord.MainApp
{
    public class AppBootStrapper: UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new MainView();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            MEFLoader mefLoader = new MEFLoader();
            mefLoader.Load();
            var analysisManagerInstance = mefLoader.GetAnalysisManager();
            var codeParser = mefLoader.GetCodeParser();
            var folderScanner = mefLoader.GetFolderScanner();
            //Container = CreateContainer();
            try
            {
                Container.RegisterInstance(typeof(IAnalysisManager), string.Empty, analysisManagerInstance, null);
                Container.RegisterInstance(typeof(ICodeParser), string.Empty, codeParser, null);
                Container.RegisterInstance(typeof(IFolderScanner), string.Empty, folderScanner, null);

                //Container.RegisterInstance<IAnalysisManager>(analysisManagerInstance);
                //Container.RegisterInstance<ICodeParser>(codeParser);
                //Container.RegisterInstance<IFolderScanner>(folderScanner);
                //UnityServiceLocatorAdapter adap = new UnityServiceLocatorAdapter(Container);
                //CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => adap);
            }
            catch (Exception ex)
            {
            }

        }


    }
}
