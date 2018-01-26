[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GlsLeague.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GlsLeague.App_Start.NinjectWebCommon), "Stop")]

namespace GlsLeague.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using GlsLeague.Repository.Interfaces;
    using GlsLeague.Repository;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICompetitionsRepository>().To<CompetitionsRepository>();
            kernel.Bind<IEventsRepository>().To<EventsRepository>();
            kernel.Bind<ICompetitionEventsRepository>().To<CompetitionEventsRepository>();
            kernel.Bind<ICompetitionEventDetailsRepository>().To<CompetitionEventDetailsRepository>();
            kernel.Bind<ICompetitorRepository>().To<CompetitorRepository>();
            kernel.Bind<ICompetitorEventsRepository>().To<CompetitorEventsRepository>();
        }        
    }
}
