using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightCore;
using LightCore.Integration.Web;

namespace NetCommunityEvents.Infrastructure
{
    public class Bootstrapper
    {
        private readonly List<IBootstrapItem> _bootstrapItems;

        private Bootstrapper()
        {
            _bootstrapItems = new List<IBootstrapItem>();
        }

        public static Bootstrapper CreateNew()
        {
            return new Bootstrapper();
        }

        private void LoadFromServiceLocator()
        {
            IContainerAccessor accessor = (IContainerAccessor)HttpContext.Current.ApplicationInstance;
            var container = accessor.Container;
            var instances = container.ResolveAll<IBootstrapItem>();

            _bootstrapItems.AddRange(instances);
        }

        public void Execute()
        {
            LoadFromServiceLocator();

            foreach (var bootstrapItem in _bootstrapItems)
            {
                bootstrapItem.Execute();
            }
        }
    }
}