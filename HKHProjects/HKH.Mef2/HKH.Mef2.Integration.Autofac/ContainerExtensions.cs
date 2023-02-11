using System.Composition.Hosting;
using Autofac;

namespace HKH.Mef2.Integration.Autofac
{
    public static class ContainerExtensions
    {
        #region Methods
        public static void EnableMef2(this ContainerBuilder builder, ContainerConfiguration mefContainer)
        {
            EnableMef2<AutofacResolver>(builder, mefContainer);
        }
        public static void EnableMef2AsDefault(this ContainerBuilder builder, ContainerConfiguration mefContainer)
        {
            EnableMef2<DefaultResolver>(builder, mefContainer);
        }

        private static void EnableMef2<Resolver>(ContainerBuilder builder, ContainerConfiguration mefContainer) where Resolver : IResolver
        {
            mefContainer.WithAssembly(typeof(IResolver).Assembly);  //register DIContainerWrapper
            builder.RegisterInstance(mefContainer)
               .AsSelf()
               .SingleInstance();
            builder.RegisterType<Resolver>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<CompositionContainer>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        #endregion
    }
}