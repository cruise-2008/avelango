using System.Data.Entity.Core.Mapping;
using System.Web.Hosting;
using System.Web.Mvc;
using Avelango.DbOrm.Implementation;
using Avelango.DbOrm.UnitOfWork;
using Avelango.Logger;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.Contracts;
using Avelango.Models.Abstractions.UnitOfWork;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace Avelango.Localisation
{
    public class WebEntityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store) {

            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifestyleTransient());
            container.Register(Component.For<IQueryableUnitOfWork, UnitOfWork>().ImplementedBy<UnitOfWork>().LifestyleTransient());

            container.Register(Component.For<IUsers>().ImplementedBy<ImpUser>().LifestyleTransient());
            container.Register(Component.For<IMails>().ImplementedBy<ImpMail>().LifestyleTransient());
            container.Register(Component.For<IEvents>().ImplementedBy<ImpEvents>().LifestyleTransient());
            container.Register(Component.For<IPortfolios>().ImplementedBy<ImpPortfolios>().LifestyleTransient());

            container.Register(Component.For<IChats>().ImplementedBy<ImpChats>().LifestyleTransient());
            container.Register(Component.For<IChatMessages>().ImplementedBy<ImpChatMessages>().LifestyleTransient());

            container.Register(Component.For<ICommonNews>().ImplementedBy<ImpCommonNews>().LifestyleTransient());
            container.Register(Component.For<ICommonAdvertising>().ImplementedBy<ImpCommonAdvertising>().LifestyleTransient());

            container.Register(Component.For<ITasks>().ImplementedBy<ImpTasks>().LifestyleTransient());
            container.Register(Component.For<ITaskAttachments>().ImplementedBy<ImpTaskAttachments>().LifestyleTransient());
            container.Register(Component.For<ITaskBids>().ImplementedBy<ImpTaskBids>().LifestyleTransient());

            container.Register(Component.For<ITaskPreWorkers>().ImplementedBy<ImpTaskPreWorkers>().LifestyleTransient());
            container.Register(Component.For<ITaskOffers>().ImplementedBy<ImpTaskOffers>().LifestyleTransient());
            container.Register(Component.For<INotifycations>().ImplementedBy<ImpNotifycations>().LifestyleTransient());

            container.Register(Component.For<IRialtos>().ImplementedBy<ImpRialtos>().LifestyleTransient());

            container.Register(Component.For<ILog>().ImplementedBy<LogManager>().LifestyleTransient().
                                         DependsOn(Dependency.OnValue("logFolder", HostingEnvironment.ApplicationPhysicalPath + @"\Logs")));

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}