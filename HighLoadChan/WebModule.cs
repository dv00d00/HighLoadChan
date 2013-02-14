namespace HighLoadChan.Presentation.Web
{
    using HighLoadChan.Core;
    using HighLoadChan.Infrastructure;
    using HighLoadChan.Storage;

    using Ninject.Modules;

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