namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Context;
    using Dropthings.Data.Repository;
    using Dropthings.Data;
    using Microsoft.Practices.Unity;
    using Dropthings.Util;
    using OmarALZabir.AspectF;
    using System.Web;

    /// <summary>
    /// Summary description for FacadeBase
    /// </summary>
    public partial class Facade : IDisposable
    {
        private Dictionary<int, IDisposable> _disposableCache = new Dictionary<int, IDisposable>();

        private T Resolve<T>(Func<T> resolver)
            where T:IDisposable
        {
            int hashcode = typeof(T).GetHashCode();
            if (_disposableCache.ContainsKey(hashcode))
                return (T)_disposableCache[hashcode];
            else
            {
                var item = resolver();
                _disposableCache.Add(hashcode, item);
                return item;
            }
        }

        #region Fields

        private IColumnRepository columnRepository { get { return Resolve<IColumnRepository>(columnRepositoryResolver); } }
        private IPageRepository pageRepository { get { return Resolve<IPageRepository>(pageRepositoryResolver); } }
        private IRoleRepository roleRepository { get { return Resolve<IRoleRepository>(roleRepositoryResolver); } }
        private IRoleTemplateRepository roleTemplateRepository { get { return Resolve<IRoleTemplateRepository>(roleTemplateRepositoryResolver); } }
        private ITokenRepository tokenRepository { get { return Resolve<ITokenRepository>(tokenRepositoryResolver); } }
        private IUserRepository userRepository { get { return Resolve<IUserRepository>(userRepositoryResolver); } }
        private IWidgetInstanceRepository widgetInstanceRepository { get { return Resolve<IWidgetInstanceRepository>(widgetInstanceRepositoryResolver); } }
        private IWidgetRepository widgetRepository { get { return Resolve<IWidgetRepository>(widgetRepositoryResolver); } }
        private IWidgetZoneRepository widgetZoneRepository { get { return Resolve<IWidgetZoneRepository>(widgetZoneRepositoryResolver); } }
        private IWidgetsInRolesRepository widgetsInRolesRepository { get { return Resolve<IWidgetsInRolesRepository>(widgetsInRolesRepositoryResolver); } }
        private IUserSettingRepository userSettingRepository { get { return Resolve<IUserSettingRepository>(userSettingRepositoryResolver); } }

        private readonly Func<IColumnRepository> columnRepositoryResolver;
        private readonly Func<IPageRepository> pageRepositoryResolver;
        private readonly Func<IRoleRepository> roleRepositoryResolver;
        private readonly Func<IRoleTemplateRepository> roleTemplateRepositoryResolver;
        private readonly Func<ITokenRepository> tokenRepositoryResolver;
        private readonly Func<IUserRepository> userRepositoryResolver;
        private readonly Func<IWidgetInstanceRepository> widgetInstanceRepositoryResolver;
        private readonly Func<IWidgetRepository> widgetRepositoryResolver;
        private readonly Func<IWidgetZoneRepository> widgetZoneRepositoryResolver;
        private readonly Func<IWidgetsInRolesRepository> widgetsInRolesRepositoryResolver;
        private readonly Func<IUserSettingRepository> userSettingRepositoryResolver;

        #endregion Fields

        #region Constructors

        public Facade()
            : this(AppContext.GetContext(HttpContext.Current))
        { 
        }

        public Facade(AppContext context) :
            this(context,
            Services.LazyGet<IColumnRepository>(),
            Services.LazyGet<IPageRepository>(),
            Services.LazyGet<IUserRepository>(),
            Services.LazyGet<IRoleRepository>(),
            Services.LazyGet<IRoleTemplateRepository>(),
            Services.LazyGet<ITokenRepository>(),
            Services.LazyGet<IWidgetRepository>(),
            Services.LazyGet<IWidgetInstanceRepository>(),
            Services.LazyGet<IWidgetZoneRepository>(),
            Services.LazyGet<IWidgetsInRolesRepository>(),
            Services.LazyGet<IUserSettingRepository>())
        {
            this.Context = context;
        }

        public Facade(AppContext context,
            Func<IColumnRepository> columnRepository,
            Func<IPageRepository> pageRepository,
            Func<IUserRepository> userRepository,
            Func<IRoleRepository> roleRepository,
            Func<IRoleTemplateRepository> roleTemplateRepository,
            Func<ITokenRepository> tokenRepository,
            Func<IWidgetRepository> widgetRepository,
            Func<IWidgetInstanceRepository> widgetInstanceRepository,
            Func<IWidgetZoneRepository> widgetZoneRepository,
            Func<IWidgetsInRolesRepository> widgetsInRolesRepository,
            Func<IUserSettingRepository> userSettingRepository)
        {
            this.columnRepositoryResolver = columnRepository;
            this.pageRepositoryResolver = pageRepository;
            this.userRepositoryResolver = userRepository;
            this.roleRepositoryResolver = roleRepository;
            this.roleTemplateRepositoryResolver = roleTemplateRepository;
            this.tokenRepositoryResolver = tokenRepository;
            this.widgetRepositoryResolver = widgetRepository;
            this.widgetInstanceRepositoryResolver = widgetInstanceRepository;
            this.widgetZoneRepositoryResolver = widgetZoneRepository;
            this.widgetsInRolesRepositoryResolver = widgetsInRolesRepository;
            this.userSettingRepositoryResolver = userSettingRepository;
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

#if MUNQ
                Services.RegisterType<ILogger>(r => new EntLibLogger());

                Services.RegisterType<IDatabase>(r => new DropthingsDataContext2());

                Services.RegisterTypeForLazyGet<IColumnRepository>( r => new ColumnRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IPageRepository>(r => new PageRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IUserRepository>(r => new UserRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IRoleRepository>(r => new RoleRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IRoleTemplateRepository>(r => new RoleTemplateRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<ITokenRepository>(r => new TokenRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IWidgetRepository>(r => new WidgetRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IWidgetInstanceRepository>(r => new WidgetInstanceRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IWidgetZoneRepository>(r => new WidgetZoneRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IWidgetsInRolesRepository>(r => new WidgetsInRolesRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));

                Services.RegisterTypeForLazyGet<IUserSettingRepository>(r => new UserSettingRepository(
                    r.Resolve<IDatabase>(), r.Resolve<ICache>()));
#else
                Services.RegisterType<ILogger, EntLibLogger>();

                var dalConstructor = new InjectionConstructor(
                        new ResolvedParameter<IDatabase>(),
                        new ResolvedParameter<ICache>());

                Services.RegisterType<IDatabase, DropthingsDataContext2>(
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
#endif
            });
        }


        private bool _Disposed = false;
        public void Dispose()
        {
            if (_Disposed) return;

            _Disposed = true;
            _disposableCache.Values.Each(d => d.Dispose());
            _disposableCache.Clear();
            _disposableCache = null;
        }

        #endregion Methods
    }
}