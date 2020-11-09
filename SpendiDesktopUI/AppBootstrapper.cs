namespace SpendiDesktopUI {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using AutoMapper;
    using Caliburn.Micro;
    using SpendiDesktopUI.Helpers;
    using SpendiDesktopUI.Library.Helpers;
    using SpendiDesktopUI.Library.Models;
    using SpendiDesktopUI.Models;
    using SpendiDesktopUI.ViewModels;

    public class AppBootstrapper : BootstrapperBase 
    {
        private SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper() 
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductUIModel>();
                cfg.CreateMap<CartItemModel, CartItemUIModel>();
            });

            var mapper = config.CreateMapper();

            _container.Instance(mapper);

            _container.Instance(_container)
                .PerRequest<IProductEndpoint, ProductEndpoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<IConfigHelper, ConfigHelper>();

            //register each ViewModel to the container
            GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel")).ToList().ForEach(viewModelType => _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) 
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}