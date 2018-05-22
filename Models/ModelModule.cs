using Autofac;

namespace Models
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AcwLogProvider>().SingleInstance();
        }
    }
}