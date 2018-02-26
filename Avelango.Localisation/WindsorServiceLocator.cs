using System;
using System.Collections.Generic;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace Avelango.Localisation
{
    public class WindsorServiceLocator : ServiceLocatorImplBase
    {
        private readonly IWindsorContainer _container;

  
        public WindsorServiceLocator(IWindsorContainer container)
        {
            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null ? _container.Resolve(key, serviceType) : _container.Resolve(serviceType);
        }


        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (object[]) _container.ResolveAll(serviceType);
        }
    }
}