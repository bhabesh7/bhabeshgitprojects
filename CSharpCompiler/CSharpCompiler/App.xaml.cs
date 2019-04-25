using Accord.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpCompiler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static IContainer _container;

        //protected virtual IContainer CreateContainer()
        //{
        //    return new Container(Rules.Default.WithAutoConcreteTypeResolution());
        //}


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //_container = CreateContainer();
            //MEFLoader mefLoader = new MEFLoader();
            //mefLoader.Load();

            //var analysisManagerInstance = mefLoader.GetAnalysisManager();
            //var codeParser = mefLoader.GetCodeParser();
            //var folderScanner = mefLoader.GetFolderScanner();

            //_container.UseInstance<IAnalysisManager>(analysisManagerInstance);
            //_container.UseInstance<ICodeParser>(codeParser);
            //_container.UseInstance<IFolderScanner>(folderScanner);

            CustomUnityBootstrapper bootStrapper = new CustomUnityBootstrapper();
            bootStrapper.Run(true);
        }
    }
}
