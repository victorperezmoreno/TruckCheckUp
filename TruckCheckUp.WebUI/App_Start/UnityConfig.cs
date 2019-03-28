using System;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.DataAccess.SQL;
using TruckCheckUp.Services;
using Unity;

namespace TruckCheckUp.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            //TableType Container
            container.RegisterType<IRepository<Driver>, SQLRepository<Driver>>();
            container.RegisterType<IRepository<DriverComment>, SQLRepository<DriverComment>>();
            container.RegisterType<IRepository<MechanicComment>, SQLRepository<MechanicComment>>();
            container.RegisterType<IRepository<PartCatalog>, SQLRepository<PartCatalog>>();
            container.RegisterType<IRepository<PartCategory>, SQLRepository<PartCategory>>();
            container.RegisterType<IRepository<PartReported>, SQLRepository<PartReported>>();
            container.RegisterType<IRepository<Situation>, SQLRepository<Situation>>();
            container.RegisterType<IRepository<Truck>, SQLRepository<Truck>>();
            container.RegisterType<IRepository<TruckManufacturer>, SQLRepository<TruckManufacturer>>();
            container.RegisterType<IRepository<TruckModel>, SQLRepository<TruckModel>>();
            
            //IdentityType Container /**Once Identity configured un-comment the below command
            //container.RegisterType<AccountController>(new InjectionConstructor());

            //LoggerType Container
            container.RegisterType<ILogger, Log4NetLogger>();

            //ValidationType Container
            container.RegisterType<IValidateUserInput, ValidateUserInput>();

            //ServicesType Container
            container.RegisterType<IDriverService, DriverService>();
            container.RegisterType<ITruckManufacturerService, TruckManufacturerService>();

        }
    }
}