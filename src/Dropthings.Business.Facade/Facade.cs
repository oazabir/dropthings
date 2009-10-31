namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Context;
    using DataAccess.Repository;
    using DataAccess;
    using Microsoft.Practices.Unity;
    using Dropthings.Util;
    using OmarALZabir.AspectF;

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
            Services.Get<IColumnRepository>(),
            Services.Get<IPageRepository>(),
            Services.Get<IUserRepository>(),
            Services.Get<IRoleRepository>(),
            Services.Get<IRoleTemplateRepository>(),
            Services.Get<ITokenRepository>(),
            Services.Get<IWidgetRepository>(),
            Services.Get<IWidgetInstanceRepository>(),
            Services.Get<IWidgetZoneRepository>(),
            Services.Get<IWidgetsInRolesRepository>(),
            Services.Get<IUserSettingRepository>())
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
                .Log(new EntLibLogger(), "Register default types in Unity")
                .Do(() =>
            {
                CacheSetup.Register();
                Services.RegisterType<ILogger, EntLibLogger>();

                var dalConstructor = new InjectionConstructor(
                        new ResolvedParameter<IDropthingsDataContext>(),
                        new ResolvedParameter<ICache>());

                Services.RegisterType<IDropthingsDataContext, DropthingsDataContext2>(
                    new InjectionConstructor(DropthingsDataContext.GetConnectionString()));
                Services.RegisterType<IColumnRepository, ColumnRepository>(dalConstructor);
                Services.RegisterType<IPageRepository, PageRepository>(dalConstructor);
                Services.RegisterType<IUserRepository, UserRepository>(dalConstructor);
                Services.RegisterType<IRoleRepository, RoleRepository>(dalConstructor);
                Services.RegisterType<IRoleTemplateRepository, RoleTemplateRepository>(dalConstructor);
                Services.RegisterType<ITokenRepository, TokenRepository>(dalConstructor);
                Services.RegisterType<IWidgetRepository, WidgetRepository>(dalConstructor);
                Services.RegisterType<IWidgetInstanceRepository, WidgetInstanceRepository>(dalConstructor);
                Services.RegisterType<IWidgetZoneRepository, WidgetZoneRepository>(dalConstructor);
                Services.RegisterType<IWidgetsInRolesRepository, WidgetsInRolesRepository>(dalConstructor);
                Services.RegisterType<IUserSettingRepository, UserSettingRepository>(dalConstructor);
            });
        }

        public void Dispose()
        {

        }

        #endregion Methods
    }
}