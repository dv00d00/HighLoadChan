[assembly: WebActivator.PreApplicationStartMethod(typeof(HighLoadChan.Presentation.MVC4.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(HighLoadChan.Presentation.MVC4.App_Start.NinjectWebCommon), "Stop")]

namespace HighLoadChan.Presentation.MVC4.App_Start
{
    using System;
    using System.Web;

    using HighLoadChan.Core;
    using HighLoadChan.Infrastructure;
    using HighLoadChan.Storage;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;

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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new WebModule());

            var readModel = kernel.Get<IReadModel>();
            var sdfa = kernel.Get<IWriteModel>();
            var asdff = kernel.Get<IMessanger>();
        }        
    }

    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IReadModel>().To<InMemmoryReadModel>().InSingletonScope();
            this.Bind<IWriteModel>().To<InMemmoryWriteModel>().InSingletonScope();
            this.Bind<IMessanger>().To<SingleMachineMessanger>().InSingletonScope();

            
        }
    }
}
