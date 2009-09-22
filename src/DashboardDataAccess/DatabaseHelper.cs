#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Dropthings.Util;

    public static class DatabaseHelper
    {
        #region Fields

        public static readonly Guid ApplicationGuid = new Guid(ApplicationID);

        public const string ApplicationID = "fd639154-299a-4a9d-b273-69dc28eb6388";
        public const string DEFAULT_CONNECTION_STRING_NAME = "DropthingsConnectionString";

        internal static Dictionary<SubsystemEnum, string> _SubsystemConnectionStringMap = new Dictionary<SubsystemEnum, string>();
        internal static Dictionary<Type, TableDef> _TableDefCache = new Dictionary<Type, TableDef>();

        #endregion Fields

        #region Enumerations

        public enum SubsystemEnum
        {
            User = 0,
            Page,
            Column,
            Widget,
            WidgetZone,
            WidgetInstance,
            Token
        }

        #endregion Enumerations

        #region Delegates

        public delegate void DataContextDelegate(DropthingsDataContext data);

        #endregion Delegates

        #region Methods

        public static void Delete<TSource>(SubsystemEnum subsystem, TSource entity)
            where TSource : class
        {
            InDataContext(subsystem, false, (data) => Delete<TSource>(entity, data) );
        }

        public static void DeleteByPK<TSource, TPK>(SubsystemEnum subsystem, TPK pk)
            where TSource : class
        {
            InDataContext(subsystem, false, (data) => DeleteByPK<TSource, TPK>(pk, data));
        }

        public static void DeleteByPK<TSource, TPK>(TPK pk, DataContext dc)
            where TSource : class
        {
            Table<TSource> table = dc.GetTable<TSource>();
            TableDef tableDef = GetTableDef<TSource>();

            dc.ExecuteCommand("DELETE FROM [" + tableDef.TableName
                + "] WHERE [" + tableDef.PKFieldName + "] = {0}",
                pk);
        }

        public static void DeleteByPK<TSource, TPK>(SubsystemEnum subsystem, TPK[] pkArray)
            where TSource : class
        {
            InDataContext(subsystem, false, (data) => DeleteByPK<TSource, TPK>(pkArray, data));
        }

        public static void DeleteByPK<TSource, TPK>(TPK[] pkArray, DataContext data)
            where TSource : class
        {
            TableDef tableDef = GetTableDef<TSource>();

            var buffer = new StringBuilder();
            buffer.Append("DELETE FROM ");

            if(tableDef.TableName.StartsWith("["))
            {
                buffer.Append(tableDef.TableName);
            }
            else
            {
                buffer.Append("[")
                      .Append(tableDef.TableName)
                      .Append("]");
            }
            buffer.Append(" WHERE [")
                .Append(tableDef.PKFieldName)
                .Append("] IN (");

            for( int i = 0; i < pkArray.Length; i ++ )
                buffer.Append('\'')
                    .Append(pkArray[i].ToString())
                    .Append('\'')
                    .Append(',');

            buffer.Length--;
            buffer.Append(')');

            data.ExecuteCommand(buffer.ToString());
        }

        public static void DeleteList<TSource>(SubsystemEnum subsystem, List<TSource> list)
            where TSource : class
        {
            InDataContext(subsystem, false, (data) => DeleteList<TSource>(list, data));
        }

        public static List<TSource> GetList<TSource>(
            SubsystemEnum subsystem,
            Func<DropthingsDataContext, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, List<TSource>>(subsystem,
                func,
                (query) => query.ToList<TSource>());
        }

        public static List<TSource> GetList<TSource, TArg0>(
            SubsystemEnum subsystem,
            TArg0 arg0,
            Func<DropthingsDataContext, TArg0, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, List<TSource>>(subsystem,
                arg0, func,
                (query) => query.ToList<TSource>());
        }

        public static List<TSource> GetList<TSource, TArg0>(
            SubsystemEnum subsystem,
            TArg0 arg0,
            Func<DropthingsDataContext, TArg0, IQueryable<TSource>> func,
            DataLoadOptions options)
        {
            return GetQueryResult<TSource, TArg0, List<TSource>>(subsystem,
                arg0, func,
                (query) => query.ToList<TSource>(), options);
        }

        public static List<TSource> GetList<TSource, TArg0, TArg1>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1,
            Func<DropthingsDataContext, TArg0, TArg1, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, TArg1, List<TSource>>(subsystem,
                arg0, arg1, func,
                (query) => query.ToList<TSource>());
        }

        public static List<TSource> GetList<TSource, TArg0, TArg1, TArg2>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1, TArg2 arg2,
            Func<DropthingsDataContext, TArg0, TArg1, TArg2, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, TArg1, TArg2, List<TSource>>(subsystem,
                arg0, arg1, arg2, func,
                (query) => query.ToList<TSource>());
        }

        public static TReturnType GetQueryResult<TSource, TReturnType>(
            SubsystemEnum subsystem,
            Func<DropthingsDataContext, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                return returnExpected(func(data));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0,
            Func<DropthingsDataContext, TArg0, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                return returnExpected(func(data, arg0));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0,
            Func<DropthingsDataContext, TArg0, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected,
            DataLoadOptions options)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                data.LoadOptions = options;
                return returnExpected(func(data, arg0));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TArg1, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1,
            Func<DropthingsDataContext, TArg0, TArg1, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                return returnExpected(func(data, arg0, arg1));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TArg1, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1,
            Func<DropthingsDataContext, TArg0, TArg1, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected,
            DataLoadOptions options)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                data.LoadOptions = options;
                return returnExpected(func(data, arg0, arg1));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TArg1, TArg2, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1, TArg2 arg2,
            Func<DropthingsDataContext, TArg0, TArg1, TArg2, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                return returnExpected(func(data, arg0, arg1, arg2));
            });
        }

        public static TReturnType GetQueryResult<TSource, TArg0, TArg1, TArg2, TReturnType>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1, TArg2 arg2,
            Func<DropthingsDataContext, TArg0, TArg1, TArg2, IQueryable<TSource>> func,
            Func<IQueryable<TSource>, TReturnType> returnExpected,
            DataLoadOptions options)
        {
            return InDataContext<TReturnType>(subsystem, true, (data) =>
            {
                data.LoadOptions = options;
                return returnExpected(func(data, arg0, arg1, arg2));
            });
        }

        public static TSource GetSingle<TSource>(
            SubsystemEnum subsystem,
            Func<DropthingsDataContext, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TSource>(subsystem,
                func,
                (query) => query.SingleOrDefault<TSource>());
        }

        public static TSource GetSingle<TSource, TArg0>(
            SubsystemEnum subsystem,
            TArg0 arg0,
            Func<DropthingsDataContext, TArg0, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, TSource>(subsystem,
                arg0, func,
                (query) => query.SingleOrDefault<TSource>());
        }

        public static TSource GetSingle<TSource, TArg0, TArg1>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1,
            Func<DropthingsDataContext, TArg0, TArg1, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, TArg1, TSource>(subsystem,
                arg0, arg1, func,
                (query) => query.SingleOrDefault<TSource>());
        }

        public static TSource GetSingle<TSource, TArg0, TArg1, TArg2>(
            SubsystemEnum subsystem,
            TArg0 arg0, TArg1 arg1, TArg2 arg2,
            Func<DropthingsDataContext, TArg0, TArg1, TArg2, IQueryable<TSource>> func)
        {
            return GetQueryResult<TSource, TArg0, TArg1, TArg2, TSource>(subsystem,
                arg0, arg1, arg2,
                func,
                (query) => query.SingleOrDefault<TSource>());
        }

        public static void InDataContext(SubsystemEnum subsystem, bool nolock, DataContextDelegate d)
        {
            
            AspectF.Define.Retry(ServiceLocator.Resolve<ILogger>()).Do(() =>
            {
                using (var data = GetDataContext(subsystem, nolock))
                    d(data);
            });
        }

        public static T InDataContext<T>(SubsystemEnum subsystem, bool nolock, Func<DropthingsDataContext, T> f)
        {
            return AspectF.Define.Retry(ServiceLocator.Resolve<ILogger>()).Return<T>(() =>
            {
                using (var data = GetDataContext(subsystem, nolock))
                    return f(data);
            });
        }

        public static TSource Insert<TSource>(SubsystemEnum subsystem, Action<TSource> populate)
            where TSource : class, new()
        {
            return InDataContext<TSource>(subsystem, false, (data) =>
            {
                TSource newObject = new TSource();

                populate(newObject);

                data.GetTable<TSource>().InsertOnSubmit(newObject);
                data.SubmitChanges();

                return newObject;
            });
        }

        public static void InsertList<TEntity, TSomething>(SubsystemEnum subsystem, IEnumerable<TSomething> items, Converter<TSomething, TEntity> converter)
            where TEntity : class
            where TSomething : class
        {
            InDataContext(subsystem, false, (data) =>
            {
                Table<TEntity> table = data.GetTable<TEntity>();
                IEnumerator<TSomething> e = items.GetEnumerator();
                while (e.MoveNext())
                {
                    TEntity newObject = converter(e.Current);
                    table.InsertOnSubmit(newObject);
                }

                data.SubmitChanges();
            });
        }

        public static void UpdateList<TEntity>(SubsystemEnum subsystem, IList<TEntity> list,
            Action<TEntity> detach,
            Action<TEntity> postAttachUpdate)
            where TEntity : class
        {
            InDataContext(subsystem, false, (data) =>
            {
                Table<TEntity> table = data.GetTable<TEntity>();
                foreach (TEntity entity in list)
                {
                    if (null != detach)
                        detach(entity);
                    table.Attach(entity, true);
                    if (null != postAttachUpdate)
                        postAttachUpdate(entity);
                }
                data.SubmitChanges(ConflictMode.FailOnFirstConflict);
            });
        }

        public static void UpdateObject<TEntity>(SubsystemEnum subsystem, TEntity obj, 
            Action<TEntity> detach,
            Action<TEntity> postAttachUpdate)
            where TEntity : class
        {
            InDataContext(subsystem, false, (data) =>
            {
                if (null != detach)
                    detach(obj);
                data.GetTable<TEntity>().Attach(obj, true);
                if( null != postAttachUpdate)
                    postAttachUpdate(obj);
                data.SubmitChanges(ConflictMode.FailOnFirstConflict);
            });
        }

        public static void UpdateObject<TEntity, TArg0>(SubsystemEnum subsystem, TArg0 arg0, Func<DropthingsDataContext, TArg0, IQueryable<TEntity>> func, 
            Action<TEntity> postAttachUpdate)
            where TEntity : class
        {
            InDataContext(subsystem, false, (data) =>
            {
                TEntity obj = GetSingle<TEntity, TArg0>(subsystem, arg0, func);
                UpdateObject<TEntity>(subsystem, obj, (o) => {}, postAttachUpdate);
            });
        }

        /// <summary>
        /// If there's a connection string specified for the subsystem, then use that 
        /// otherwise use the first connection string as the default for the subsystem 
        /// Connection string name must be in this format: SubsystemNameConnectionString           
        /// </summary>
        /// <param name="subsystem"></param>
        /// <returns></returns>
        internal static string GetConnectionString(SubsystemEnum subsystem)
        {
            if (_SubsystemConnectionStringMap.ContainsKey(subsystem))
                return _SubsystemConnectionStringMap[subsystem];

            lock (_SubsystemConnectionStringMap)
            {
                if (_SubsystemConnectionStringMap.ContainsKey(subsystem))
                {
                    return _SubsystemConnectionStringMap[subsystem];
                }
                else
                {
                    var connectionStringSetting = ConfigurationManager.ConnectionStrings[subsystem.ToString() + "ConnectionString"] ??
                        ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_STRING_NAME];
                    _SubsystemConnectionStringMap.Add(subsystem, connectionStringSetting.ConnectionString);
                    return connectionStringSetting.ConnectionString;
                }
            }
        }

        /// <summary>
        /// If there's a connection string specified for the subsystem, then use that 
        /// otherwise use the first connection string as the default for the subsystem 
        /// Connection string name must be in this format: SubsystemNameConnectionString           
        /// </summary>
        /// <param name="subsystem"></param>
        /// <returns></returns>
        internal static string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// Creates a disconnected DataContext which does not support deferred loading
        /// </summary>
        /// <param name="subsystem"></param>
        /// <returns></returns>
        internal static DropthingsDataContext2 GetDataContext(SubsystemEnum subsystem, bool nolock)
        {
            return GetDataContext(GetConnectionString(subsystem), nolock);
        }

        /// <summary>
        /// Creates a disconnected DataContext which does not support deferred loading from Connection String
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="nolock"></param>
        /// <returns></returns>
        internal static DropthingsDataContext2 GetDataContext(string connectionString, bool nolock)
        {
            var context = new DropthingsDataContext2(connectionString);
            context.Log = new DebugStreamWriter();
            context.DeferredLoadingEnabled = false;

            if (nolock)
            {
                context.Connection.Open();
                using (var command = context.Connection.CreateCommand())
                {
                    command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;";
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            return context;
        }

        internal static TableDef GetTableDef<TEntity>()
            where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (!_TableDefCache.ContainsKey(entityType))
            {
                lock(_TableDefCache)
                {
                    if (!_TableDefCache.ContainsKey(entityType))
                    {
                        object[] attributes = entityType.GetCustomAttributes(typeof(TableAttribute), true);
                        string tableName = (attributes[0] as TableAttribute).Name;
                        if (tableName.StartsWith("dbo."))
                            tableName = tableName.Substring("dbo.".Length);
                        tableName = tableName.TrimStart('[').TrimEnd(']');

                        string pkFieldName = "ID";

                        // Find the property which is the primary key so that we can find the
                        // primary key field name in database
                        foreach (PropertyInfo prop in entityType.GetProperties())
                        {
                            object[] columnAttributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                            if (columnAttributes.Length > 0)
                            {
                                ColumnAttribute columnAtt = columnAttributes[0] as ColumnAttribute;
                                if (columnAtt.IsPrimaryKey)
                                    pkFieldName = columnAtt.Storage.TrimStart('_');
                            }
                        }

                        var tableDef = new TableDef { TableName = tableName, PKFieldName = pkFieldName };
                        _TableDefCache.Add(entityType, tableDef);
                        return tableDef;
                    }
                    else
                    {
                        return _TableDefCache[entityType];
                    }
                }
            }
            else
            {
                return _TableDefCache[entityType];
            }
        }

        private static void Delete<TSource>(TSource entity, DropthingsDataContext data)
            where TSource : class
        {
            (new Action<Table<TSource>>((table) =>
            {
                table.Attach(entity);
                table.DeleteOnSubmit(entity);
            }))(data.GetTable<TSource>());

            data.SubmitChanges();
        }

        private static void DeleteList<TSource>(List<TSource> list, DropthingsDataContext data)
            where TSource : class
        {
            (new Action<Table<TSource>>((table) =>
            {
                table.AttachAll(list);
                table.DeleteAllOnSubmit(list);
            }))(data.GetTable<TSource>());

            data.SubmitChanges();
        }

        #endregion Methods

        #region Nested Types

        private class DebugStreamWriter : StreamWriter
        {
            #region Constructors

            public DebugStreamWriter()
                : base(new MemoryStream())
            {
            }

            #endregion Constructors

            #region Methods

            public override void WriteLine(string value)
            {
                Debug.WriteLine(value);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }

    internal class TableDef
    {
        #region Fields

        public string PKFieldName;
        public string TableName;

        #endregion Fields
    }
}