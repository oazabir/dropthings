#define MUNQ

namespace Dropthings.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using Microsoft.Practices.Unity;
    using Munq.DI;
    using Munq.DI.LifetimeManagers;

    public class Services
    {
        #region Fields

        
#if MUNQ
        private static readonly ILifetimeManager _containerLifetime = 
            new ContainerLifetime();
        private static readonly Container _container = new Container();
            
#else
        private static readonly IUnityContainer _container = new UnityContainer();
#endif
        #endregion Fields

        #region Methods

        public static void Dispose()
        {
            if (null != _container)
            {
                _container.Dispose();
            }
        }

#if MUNQ

        public static IRegistration RegisterType<T>(Func<Container, T> register)
            where T:class
        {
            return _container.Register<T>(register);
        }

        public static IRegistration RegisterInstance<T>(Func<Container, T> register)
            where T:class
        {
            return _container.RegisterInstance<T>(register(_container))
                .WithLifetimeManager(_containerLifetime);
        }

        public static IRegistration RegisterTypeForLazyGet<T>(Func<Container, T> register)
        {
            Func<T> lazyResolve = () => register(_container);
            return _container.RegisterInstance<Func<T>>(lazyResolve);
        }


        public static T Get<T>()
            where T:class
        {
            return _container.Resolve<T>();
        }

        public static Func<T> LazyGet<T>()
            where T : class
        {
            return _container.Resolve<Func<T>>();
        }

#else
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

#endif



        #endregion Methods
    }
}