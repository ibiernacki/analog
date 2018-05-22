using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using ViewModels;
using Views;
using Models;
using System.Windows.Input;
using ViewModels.Services;
using Views.Input;

namespace Launcher
{
    public class AutofacBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        public AutofacBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<DefaultFoldingService>().As<IFoldingService>().SingleInstance();

            builder.RegisterAssemblyModules(typeof(ViewModelModule).Assembly);
            builder.RegisterAssemblyModules(typeof(ViewModule).Assembly);
            builder.RegisterAssemblyModules(typeof(ModelModule).Assembly);

            _container = builder.Build();

            ConfigureCaliburnGesturesExtension();
        }

        private void ConfigureCaliburnGesturesExtension()
        {
            var defaultCreateTrigger = Parser.CreateTrigger;

            Parser.CreateTrigger = (target, triggerText) =>
            {
                if (triggerText == null)
                {
                    return defaultCreateTrigger(target, null);
                }

                var triggerDetail = triggerText
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty);

                var splits = triggerDetail.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                switch (splits[0])
                {
                    case "Key":
                        var key = (Key)Enum.Parse(typeof(Key), splits[1], true);
                        return new KeyTrigger { Key = key };

                    case "Gesture":
                        var mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };
                }

                return defaultCreateTrigger(target, triggerText);
            };
        }

        protected override object GetInstance(Type service, string key)
        {
            return key == null
                ? _container.Resolve(service)
                : _container.ResolveNamed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var collectionType = typeof(IEnumerable<>).MakeGenericType(service);
            return (IEnumerable<object>)_container.Resolve(collectionType);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
                typeof(ViewModelModule).Assembly,
                typeof(ViewModule).Assembly,
            };
        }
    }
}