namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Context;
    using Business.Container;
    using DataAccess.Repository;
    using DataAccess;
    using Microsoft.Practices.Unity;
    using Dropthings.Util;

    /// <summary>
    /// Summary description for FacadeBase
    /// </summary>
    public partial class Facade : IDisposable
    {
        #region Fields

        private IColumnRepository columnRepository;
        private IPageRepository pageRepository;
        private IRoleRepository roleRepository;
        private IRoleTemplateRepository roleTemplateRepository;
        private ITokenRepository tokenRepository;
        private IUserRepository userRepository;
        private IWidgetInstanceRepository widgetInstanceRepository;
        private IWidgetRepository widgetRepository;
        private IWidgetZoneRepository widgetZoneRepository;
        private IWidgetsInRolesRepository widgetsInRolesRepository;
        private IUserSettingRepository userSettingRepository;

        #endregion Fields

        #region Constructors

        public Facade(AppContext context) :
            this(context,
            ServiceLocator.Resolve<IColumnRepository>(),
            ServiceLocator.Resolve<IPageRepository>(),
            ServiceLocator.Resolve<IUserRepository>(),
            ServiceLocator.Resolve<IRoleRepository>(),
            ServiceLocator.Resolve<IRoleTemplateRepository>(),
            ServiceLocator.Resolve<ITokenRepository>(),
            ServiceLocator.Resolve<IWidgetRepository>(),
            ServiceLocator.Resolve<IWidgetInstanceRepository>(),
            ServiceLocator.Resolve<IWidgetZoneRepository>(),
            ServiceLocator.Resolve<IWidgetsInRolesRepository>(),
            ServiceLocator.Resolve<IUserSettingRepository>())
        {
            this.Context = context;
        }

        public Facade(AppContext context,
            IColumnRepository columnRepository,
            IPageRepository pageRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRoleTemplateRepository roleTemplateRepository,
            ITokenRepository tokenRepository,
            IWidgetRepository widgetRepository,
            IWidgetInstanceRepository widgetInstanceRepository,
            IWidgetZoneRepository widgetZoneRepository,
            IWidgetsInRolesRepository widgetsInRolesRepository,
            IUserSettingRepository userSettingRepository)
        {
            this.columnRepository = columnRepository;
            this.pageRepository = pageRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.roleTemplateRepository = roleTemplateRepository;
            this.tokenRepository = tokenRepository;
            this.widgetRepository = widgetRepository;
            this.widgetInstanceRepository = widgetInstanceRepository;
            this.widgetZoneRepository = widgetZoneRepository;
            this.widgetsInRolesRepository = widgetsInRolesRepository;
            this.userSettingRepository = userSettingRepository;
        }

        #endregion Constructors

        #region Properties

        public AppContext Context
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public static void BootStrap()
        {
            AspectF.Define
                .Log(Logger.Writer, "Register default types in Unity")
                .Do(() =>
            {
                ServiceLocator.RegisterType<IDropthingsDataContext, DropthingsDataContext2>();
                ServiceLocator.InjectIntoConstructor<DropthingsDataContext2>(); // dummy injection for empty constructor
                ServiceLocator.RegisterType<IColumnRepository, ColumnRepository>();
                ServiceLocator.RegisterType<IPageRepository, PageRepository>();
                ServiceLocator.RegisterType<IUserRepository, UserRepository>();
                ServiceLocator.RegisterType<IRoleRepository, RoleRepository>();
                ServiceLocator.RegisterType<IRoleTemplateRepository, RoleTemplateRepository>();
                ServiceLocator.RegisterType<ITokenRepository, TokenRepository>();
                ServiceLocator.RegisterType<IWidgetRepository, WidgetRepository>();
                ServiceLocator.RegisterType<IWidgetInstanceRepository, WidgetInstanceRepository>();
                ServiceLocator.RegisterType<IWidgetZoneRepository, WidgetZoneRepository>();
                ServiceLocator.RegisterType<IWidgetsInRolesRepository, WidgetsInRolesRepository>();
                ServiceLocator.RegisterType<IUserSettingRepository, UserSettingRepository>();
            });
        }

        public void Dispose()
        {

        }

        #endregion Methods
    }
}