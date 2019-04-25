using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism;
using Accord.Interfaces;
using Prism.Modularity;
using Prism.Unity;
using Unity;
using Prism.Regions;

namespace CSharpCompiler
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomUnityBootstrapper : UnityBootstrapper
#pragma warning restore CS0618 // Type or member is obsolete
    {
        //IContainer ContainerInstance;
        //protected override IContainerExtension CreateContainerExtension()
        //{
        //    throw new NotImplementedException();
        //}

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

       
        //protected virtual IContainer CreateContainer()
        //{
        //    return new Container(Rules.Default.WithAutoConcreteTypeResolution());
        //}


        protected override void InitializeShell()
        {
            base.InitializeShell();            
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }
       
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            //ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            //moduleCatalog.AddModule(typeof(Accord.ViewModule.AccordBasicModule));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            
            //MEFLoader mefLoader = new MEFLoader();
            //mefLoader.Load();

            //var analysisManagerInstance = mefLoader.GetAnalysisManager();
            //var codeParser = mefLoader.GetCodeParser();
            //var folderScanner = mefLoader.GetFolderScanner();
            //Container = CreateContainer();
            //try
            //{
            //    Container.RegisterInstance<IAnalysisManager>(analysisManagerInstance);
            //    Container.RegisterInstance<ICodeParser>(codeParser);
            //    Container.RegisterInstance<IFolderScanner>(folderScanner);
            //    UnityServiceLocatorAdapter adap = new UnityServiceLocatorAdapter(Container);                
            //    CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => adap );
            //}
            //catch (Exception ex)
            //{                
            //}

            //this.Shell = CreateShell();
            //Container.RegisterType<IRegionManager>();
            //RegionManager.SetRegionManager(this.Shell, Container.Resolve<IRegionManager>());
            //RegionManager.UpdateRegions();
            ////InitializeShell();

        }     

       
    }
}
