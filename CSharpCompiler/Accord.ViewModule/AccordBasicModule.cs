using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Regions;

namespace Accord.ViewModule
{
    public class AccordBasicModule : IModule
    {
        IRegionManager _regionManager;
        public AccordBasicModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(AccordView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            
        }
    }
}
