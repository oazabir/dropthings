namespace Dropthings.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using Microsoft.Practices.Unity;

    public class Services
    {
        #region Fields

        private static readonly IUnityContainer _container = new UnityContainer();

        #endregion Fields

        #region Methods

        public static void Dispose()
        {
            if (null != _container)
            {
                _container.Dispose();
            }
        }

        public static void RegisterInstanceExternalLifetime<TInterface>(TInterface instance)
        {
            _container.RegisterInstance<TInterface>(instance, new ExternallyControlledLifetimeManager());
        }

        public static void RegisterInstanceExternalLifetime<TInterface>(string name, TInterface instance)
        {
            _container.RegisterInstance<TInterface>(name, instance, new ExternallyControlledLifetimeManager());
        }

        public static void RegisterInstancePerThread<TInterface>(TInterface instance)
        {
            _container.RegisterInstance<TInterface>(instance, new PerThreadLifetimeManager());
        }

        public static void RegisterInstancePerThread<TInterface>(string name, TInterface instance)
        {
            _container.RegisterInstance<TInterface>(name, instance, new PerThreadLifetimeManager());
        }

        public static void RegisterType<T>(params InjectionMember[] injectionMembers)
        {
            _container.RegisterType<T>(injectionMembers);
        }

        public static void RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            _container.RegisterType<TFrom,TTo>(injectionMembers);
        }

        public static void RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(name, injectionMembers);
        }

        public static void RegisterType<T>(string name, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType<T>(name, injectionMembers);
        }

        public static void RegisterTypePerThread<T>(params InjectionMember[] injectionMembers)
        {
            _container.RegisterType<T>(new PerThreadLifetimeManager(), injectionMembers);
        }

        public static void RegisterTypePerThread<TFrom, TTo>(params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(new PerThreadLifetimeManager(), injectionMembers);
        }

        public static void RegisterTypePerThread<TFrom, TTo>(string name, params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(name, new PerThreadLifetimeManager(), injectionMembers);
        }

        public static void RegisterTypePerThread<T>(string name, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType<T>(name, new PerThreadLifetimeManager(), injectionMembers);
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }

        public static T Get<T>(string name)
        {
            return _container.Resolve<T>(name);
        }

        public static object Get(Type t)
        {
            return _container.Resolve(t);
        }

        public static object Get(Type t, string name)
        {
            return _container.Resolve(t, name);
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        public static IEnumerable<object> GetAll(Type t)
        {
            return _container.ResolveAll(t);
        }

        public static void Teardown(object o)
        {
            _container.Teardown(o);
        }

        public static void InjectIntoConstructor<T>(params object[] parameters) 
        {
            _container.Configure<InjectedMembers>().ConfigureInjectionFor<T>(new InjectionConstructor(parameters));
        }

        #endregion Methods
    }
}