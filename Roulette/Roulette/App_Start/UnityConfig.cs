using Roulette.DataAccess;
using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.DataAccess.Services;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace Roulette
{
    public static class UnityConfig
    {
        public static UnityContainer RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<RouletteDbContext>(new InjectionFactory(c => new RouletteDbContext()));
            container.RegisterType<IRepository<Colors>, Repository<Colors>>();
            container.RegisterType<IRepository<Numbers>, Repository<Numbers>>();
            container.RegisterType<IRepository<Logs>, Repository<Logs>>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.ResolveAll<IRepository<Colors>>();
            container.ResolveAll<IRepository<Numbers>>();
            container.ResolveAll<IRepository<Logs>>();
            container.ResolveAll<ApiController>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            return container;
        }
    }
}