namespace Dropthings.DataAccess
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public interface IDropthingsDataContext
    {
        #region Properties

        IQueryable<Column> ColumnsSource
        {
            get;
        }

        IQueryable<Page> PagesSource
        {
            get;
        }

        IQueryable<RoleTemplate> RoleTemplatesSource
        {
            get;
        }

        IQueryable<Token> TokensSource
        {
            get;
        }

        IQueryable<UserSetting> UserSettingsSource
        {
            get;
        }

        IQueryable<WidgetInstance> WidgetInstancesSource
        {
            get;
        }

        IQueryable<WidgetZone> WidgetZonesSource
        {
            get;
        }

        IQueryable<WidgetsInRole> WidgetsInRolesSource
        {
            get;
        }

        IQueryable<Widget> WidgetsSource
        {
            get;
        }

        IQueryable<aspnet_Role> aspnet_RolesSource
        {
            get;
        }

        IQueryable<aspnet_UsersInRole> aspnet_UsersInRolesSource
        {
            get;
        }

        IQueryable<aspnet_User> aspnet_UsersSource
        {
            get;
        }

        #endregion Properties

        #region Methods

        void Delete<TSource>(DropthingsDataContext.SubsystemEnum subsystem, TSource entity)
            where TSource : class;

        void DeleteByPK<TSource, TPK>(DropthingsDataContext.SubsystemEnum subsystem, TPK pk)
            where TSource : class;

        void DeleteByPK<TSource, TPK>(TPK[] pkArray, System.Data.Linq.DataContext data)
            where TSource : class;

        void DeleteByPK<TSource, TPK>(DropthingsDataContext.SubsystemEnum subsystem, TPK[] pkArray)
            where TSource : class;

        void DeleteByPK<TSource, TPK>(TPK pk, System.Data.Linq.DataContext dc)
            where TSource : class;

        void DeleteList<TSource>(DropthingsDataContext.SubsystemEnum subsystem, System.Collections.Generic.List<TSource> list)
            where TSource : class;

        void Dispose();

        System.Collections.Generic.List<TSource> GetList<TSource>(DropthingsDataContext.SubsystemEnum subsystem, Func<DropthingsDataContext, System.Linq.IQueryable<TSource>> func);

        System.Collections.Generic.List<TSource> GetList<TSource, TArg0>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TSource>> func, System.Data.Linq.DataLoadOptions options);

        System.Collections.Generic.List<TSource> GetList<TSource, TArg0, TArg1>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, Func<DropthingsDataContext, TArg0, TArg1, System.Linq.IQueryable<TSource>> func);

        System.Collections.Generic.List<TSource> GetList<TSource, TArg0, TArg1, TArg2>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, TArg2 arg2, Func<DropthingsDataContext, TArg0, TArg1, TArg2, System.Linq.IQueryable<TSource>> func);

        System.Collections.Generic.List<TSource> GetList<TSource, TArg0>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TSource>> func);

        TReturnType GetQueryResult<TSource, TArg0, TArg1, TArg2, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, TArg2 arg2, Func<DropthingsDataContext, TArg0, TArg1, TArg2, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected, System.Data.Linq.DataLoadOptions options);

        TReturnType GetQueryResult<TSource, TArg0, TArg1, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, Func<DropthingsDataContext, TArg0, TArg1, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected);

        TReturnType GetQueryResult<TSource, TArg0, TArg1, TArg2, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, TArg2 arg2, Func<DropthingsDataContext, TArg0, TArg1, TArg2, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected);

        TReturnType GetQueryResult<TSource, TArg0, TArg1, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, Func<DropthingsDataContext, TArg0, TArg1, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected, System.Data.Linq.DataLoadOptions options);

        TReturnType GetQueryResult<TSource, TArg0, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected, System.Data.Linq.DataLoadOptions options);

        TReturnType GetQueryResult<TSource, TArg0, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected);

        TReturnType GetQueryResult<TSource, TReturnType>(DropthingsDataContext.SubsystemEnum subsystem, Func<DropthingsDataContext, System.Linq.IQueryable<TSource>> func, Func<System.Linq.IQueryable<TSource>, TReturnType> returnExpected);

        TSource GetSingle<TSource, TArg0, TArg1>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, Func<DropthingsDataContext, TArg0, TArg1, System.Linq.IQueryable<TSource>> func);

        TSource GetSingle<TSource, TArg0>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TSource>> func);

        TSource GetSingle<TSource>(DropthingsDataContext.SubsystemEnum subsystem, Func<DropthingsDataContext, System.Linq.IQueryable<TSource>> func);

        TSource GetSingle<TSource, TArg0, TArg1, TArg2>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, TArg1 arg1, TArg2 arg2, Func<DropthingsDataContext, TArg0, TArg1, TArg2, System.Linq.IQueryable<TSource>> func);

        T InDataContext<T>(DropthingsDataContext.SubsystemEnum subsystem, bool nolock, Func<DropthingsDataContext, T> f);

        void InDataContext(DropthingsDataContext.SubsystemEnum subsystem, bool nolock, DropthingsDataContext.DataContextDelegate d);

        TSource Insert<TSource>(DropthingsDataContext.SubsystemEnum subsystem, Action<TSource> populate)
            where TSource : class, new();

        void InsertList<TEntity, TSomething>(DropthingsDataContext.SubsystemEnum subsystem, System.Collections.Generic.IEnumerable<TSomething> items, Converter<TSomething, TEntity> converter)
            where TEntity : class
            where TSomething : class;

        void UpdateList<TEntity>(DropthingsDataContext.SubsystemEnum subsystem, IEnumerable<TEntity> list, Action<TEntity> detach, Action<TEntity> postAttachUpdate)
            where TEntity : class;

        void UpdateObject<TEntity>(DropthingsDataContext.SubsystemEnum subsystem, TEntity obj, Action<TEntity> detach, Action<TEntity> postAttachUpdate)
            where TEntity : class;

        void UpdateObject<TEntity, TArg0>(DropthingsDataContext.SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, System.Linq.IQueryable<TEntity>> func, Action<TEntity> postAttachUpdate)
            where TEntity : class;

        #endregion Methods
    }
}