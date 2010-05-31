using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.EntityClient;

namespace Dropthings.Data
{
    public class DropthingsDataContext2 : DropthingsDataContext, Dropthings.Data.IDatabase
    {
        internal const string ApplicationID = "fd639154-299a-4a9d-b273-69dc28eb6388";
        internal static readonly Guid ApplicationGuid = new Guid(ApplicationID);
        internal const string DEFAULT_CONNECTION_STRING_NAME = "DropthingsDataContext";
        public readonly static string ConnectionString = GetConnectionString();

        private static Dictionary<string, Action<DropthingsDataContext, object>> 
            _AddToMethodCache =
                new Dictionary<string, Action<DropthingsDataContext, object>>();

        private static Dictionary<string, Action<DropthingsDataContext, object>>
            _AttachMethodCache =
                new Dictionary<string, Action<DropthingsDataContext, object>>();

        public DropthingsDataContext2() : this(DropthingsDataContext2.ConnectionString) 
        {
            
        }

        public DropthingsDataContext2(string connectionString) : base(connectionString)
        {
            //this.AspNetMemberships.MergeOption =
            //this.AspNetProfiles.MergeOption =
            //this.AspNetRole.MergeOption =
            //this.AspNetUser.MergeOption =
            //this.Columns.MergeOption =
            //this.Pages.MergeOption =
            //this.RoleTemplates.MergeOption =
            //this.Tokens.MergeOption =
            //this.UserSettings.MergeOption =
            //this.Widgets.MergeOption =
            //this.WidgetInstances.MergeOption =
            //this.WidgetsInRolesSet.MergeOption =
            //this.WidgetZones.MergeOption = System.Data.Objects.MergeOption.NoTracking;

            if (_AddToMethodCache.Count == 0)
            {
                lock (_AddToMethodCache)
                {
                    if (_AddToMethodCache.Count == 0)
                    {
                        _AddToMethodCache.Add(typeof(AspNetMembership).Name,
                            (context, entity) => context.AddToAspNetMemberships(entity as AspNetMembership));
                        _AddToMethodCache.Add(typeof(AspNetProfile).Name,
                            (context, entity) => context.AddToAspNetProfiles(entity as AspNetProfile));
                        _AddToMethodCache.Add(typeof(AspNetRole).Name,
                            (context, entity) => context.AddToAspNetRoles(entity as AspNetRole));
                        _AddToMethodCache.Add(typeof(AspNetUser).Name,
                            (context, entity) => context.AddToAspNetUsers(entity as AspNetUser));
                        _AddToMethodCache.Add(typeof(Column).Name,
                            (context, entity) => context.AddToColumns(entity as Column));
                        _AddToMethodCache.Add(typeof(Page).Name,
                            (context, entity) => context.AddToPages(entity as Page));
                        _AddToMethodCache.Add(typeof(RoleTemplate).Name,
                            (context, entity) => context.AddToRoleTemplates(entity as RoleTemplate));
                        _AddToMethodCache.Add(typeof(Token).Name,
                            (context, entity) => context.AddToTokens(entity as Token));
                        _AddToMethodCache.Add(typeof(UserSetting).Name,
                            (context, entity) => context.AddToUserSettings(entity as UserSetting));
                        _AddToMethodCache.Add(typeof(Widget).Name,
                            (context, entity) => context.AddToWidgets(entity as Widget));
                        _AddToMethodCache.Add(typeof(WidgetInstance).Name,
                            (context, entity) => context.AddToWidgetInstances(entity as WidgetInstance));
                        _AddToMethodCache.Add(typeof(WidgetsInRoles).Name,
                            (context, entity) => context.AddToWidgetsInRolesSet(entity as WidgetsInRoles));
                        _AddToMethodCache.Add(typeof(WidgetZone).Name,
                            (context, entity) => context.AddToWidgetZones(entity as WidgetZone));
                    }
                }
            }

            if (_AttachMethodCache.Count == 0)
            {
                lock (_AttachMethodCache)
                {
                    if (_AttachMethodCache.Count == 0)
                    {
                        _AttachMethodCache.Add(typeof(AspNetMembership).Name,
                            (context, entity) => context.AttachTo("AspNetMemberships", entity));
                        _AttachMethodCache.Add(typeof(AspNetProfile).Name,
                            (context, entity) => context.AttachTo("AspNetProfiles", entity));
                        _AttachMethodCache.Add(typeof(AspNetRole).Name,
                            (context, entity) => context.AttachTo("AspNetRoles", entity));
                        _AttachMethodCache.Add(typeof(AspNetUser).Name,
                            (context, entity) => context.AttachTo("AspNetUsers", entity));
                        _AttachMethodCache.Add(typeof(Column).Name,
                            (context, entity) => context.AttachTo("Columns", entity));
                        _AttachMethodCache.Add(typeof(Page).Name,
                            (context, entity) => context.AttachTo("Pages", entity));
                        _AttachMethodCache.Add(typeof(RoleTemplate).Name,
                            (context, entity) => context.AttachTo("RoleTemplates",entity));
                        _AttachMethodCache.Add(typeof(Token).Name,
                            (context, entity) => context.AttachTo("Tokens", entity));
                        _AttachMethodCache.Add(typeof(UserSetting).Name,
                            (context, entity) => context.AttachTo("UserSettings", entity));
                        _AttachMethodCache.Add(typeof(Widget).Name,
                            (context, entity) => context.AttachTo("Widgets", entity));
                        _AttachMethodCache.Add(typeof(WidgetInstance).Name,
                            (context, entity) => context.AttachTo("WidgetInstances", entity));
                        _AttachMethodCache.Add(typeof(WidgetsInRoles).Name,
                            (context, entity) => context.AttachTo("WidgetsInRolesSet", entity));
                        _AttachMethodCache.Add(typeof(WidgetZone).Name,
                            (context, entity) => context.AttachTo("WidgetZones", entity));
                    }
                }
            }
        }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_STRING_NAME].ConnectionString;
        }

        public int ExecuteFunction(string functionName, params EntityParameter[] parameters)             
        {
            if (base.Connection.State != ConnectionState.Open)
                base.Connection.Open();

            using (var command = (base.Connection as EntityConnection).CreateCommand())
            {
                command.CommandText = this.DefaultContainerName + "." + functionName;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);
                command.Parameters.Add("RESULT", DbType.Int32).Direction = ParameterDirection.Output;

                var result = command.ExecuteScalar();

                return (int)result;
            }
        }

        private IQueryable<TReturnType> RunQuery<TReturnType>(IQueryable<TReturnType> query)
        {
            var objectQuery = query as ObjectQuery<TReturnType>;
            objectQuery.EnablePlanCaching = true;
            return objectQuery.Execute(MergeOption.NoTracking).AsQueryable();
        }
        public IQueryable<TReturnType> Query<TReturnType>(Func<DropthingsDataContext, IQueryable<TReturnType>> query)
        {
            return RunQuery(query(this));
        }

        public IQueryable<TReturnType> Query<Arg0, TReturnType>(Func<DropthingsDataContext, Arg0, IQueryable<TReturnType>> query, Arg0 arg0)
        {
            return RunQuery(query(this, arg0));
        }

        public IQueryable<TReturnType> Query<Arg0, Arg1, TReturnType>(Func<DropthingsDataContext, Arg0, Arg1, IQueryable<TReturnType>> query, Arg0 arg0, Arg1 arg1)
        {
            return RunQuery(query(this, arg0, arg1));
        }

        public IQueryable<TReturnType> Query<Arg0, Arg1, Arg2, TReturnType>(Func<DropthingsDataContext, Arg0, Arg1, Arg2, IQueryable<TReturnType>> query, Arg0 arg0, Arg1 arg1, Arg2 arg2)
        {
            return RunQuery(query(this, arg0, arg1, arg2));
        }

        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : EntityObject
        {
            AddTo<TEntity>(entity);
            this.SaveChanges(true);
            // Without this, attaching new entity of same type in same context fails.
            this.Detach(entity);
            return entity;
        }
        public TEntity Insert<TParent, TEntity>(
            TParent parent,
            Action<TParent, TEntity> addChildToParent,
            TEntity entity)
            where TEntity : EntityObject
            where TParent : EntityObject
        {
            AddTo<TParent, TEntity>(parent, addChildToParent, entity);
            this.SaveChanges();
            this.AcceptAllChanges();
            // Without this, consequtive insert using same parent in same context fails.
            this.Detach(parent);
            // Without this, attaching new entity of same type in same context fails.
            this.Detach(entity);
            return entity;
        }

        public TEntity Insert<TParent1, TParent2, TEntity>(
            TParent1 parent1, TParent2 parent2,
            Action<TParent1, TEntity> addChildToParent1,
            Action<TParent2, TEntity> addChildToParent2,
            TEntity entity)
            where TEntity : EntityObject
            where TParent1 : EntityObject
            where TParent2 : EntityObject
        {
            AddTo<TParent1, TParent2, TEntity>(parent1, parent2, addChildToParent1, addChildToParent2, entity);

            this.SaveChanges(true);

            // Without this, consequtive insert using same parent in same context fails.
            this.Detach(parent1);
            // Without this, consequtive insert using same parent in same context fails.
            this.Detach(parent2);
            // Without this, attaching new entity of same type in same context fails.
            this.Detach(entity);
            return entity;
        }

        public void InsertList<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : EntityObject
        {
            entities.Each(entity => Attach<TEntity>(entity));
            this.SaveChanges(true);

        }
        public void InsertList<TParent, TEntity>(
            TParent parent,
            Action<TParent, TEntity> addChildToParent,
            IEnumerable<TEntity> entities)
            where TEntity : EntityObject
            where TParent : EntityObject
        {
            entities.Each(entity => AddTo<TParent, TEntity>(parent, addChildToParent, entity));
            this.SaveChanges(true);
        }

        public void InsertList<TParent1, TParent2, TEntity>(
            TParent1 parent1, TParent2 parent2,
            Action<TParent1, TEntity> addChildToParent1,
            Action<TParent2, TEntity> addChildToParent2,
            IEnumerable<TEntity> entities)
            where TEntity : EntityObject
            where TParent1 : EntityObject
            where TParent2 : EntityObject
        {
            entities.Each(entity => AddTo<TParent1, TParent2, TEntity>(parent1, parent2,
                addChildToParent1, addChildToParent2, entity));

            this.SaveChanges();
            this.AcceptAllChanges();
        }

        private void AddTo<TParent, TEntity>(TParent parent, Action<TParent, TEntity> addChildToParent, TEntity entity)
            where TEntity : EntityObject
            where TParent : EntityObject
        {
            Attach<TParent>(parent);
            //AddTo<TEntity>(entity);            
            addChildToParent(parent, entity);
        }

        private void AddTo<TParent1, TParent2, TEntity>(TParent1 parent1, TParent2 parent2, Action<TParent1, TEntity> addChildToParent1, Action<TParent2, TEntity> addChildToParent2, TEntity entity)
            where TEntity : EntityObject
            where TParent1 : EntityObject
            where TParent2 : EntityObject
        {
            Attach<TParent1>(parent1);
            Attach<TParent2>(parent2);

            addChildToParent1(parent1, entity);
            addChildToParent2(parent2, entity);

            //AddTo<TEntity>(entity);
        }

        public void Attach<TEntity>(TEntity entity)
            where TEntity : EntityObject
        {
            if (entity.EntityState != EntityState.Detached)
                return;
            // Let's see if the same entity with same key values are already there
            ObjectStateEntry entry;
            if (ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
            }
            else
            {
                _AttachMethodCache[typeof(TEntity).Name](this, entity);
            }
        }
        public void AddTo<TEntity>(TEntity entity)
        {
            _AddToMethodCache[typeof(TEntity).Name](this, entity);
        }


        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : EntityObject
        {
            //Attach<TEntity>(entity);
            AttachUpdated(entity);
            //SetEntryModified(this, entity);
            this.SaveChanges(true);
            
            return entity;
        }

        public void UpdateList<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : EntityObject
        {
            foreach (TEntity entity in entities)
            {
                //Attach<TEntity>(entity);
                AttachUpdated(entity);
                //SetEntryModified(this, entity);
            }

            this.SaveChanges(true);
        }

        public void AttachUpdated(EntityObject objectDetached)
        {
            if (objectDetached.EntityState == EntityState.Detached)
            {
                object currentEntityInDb = null;
                if (this.TryGetObjectByKey(objectDetached.EntityKey, out currentEntityInDb))
                {
                    this.ApplyPropertyChanges(objectDetached.EntityKey.EntitySetName, objectDetached);
                    //(CDLTLL)Apply property changes to all referenced entities in context
                    ApplyReferencePropertyChanges((IEntityWithRelationships)objectDetached,
                        (IEntityWithRelationships)currentEntityInDb); //Custom extensor method
                }
                else
                {
                    throw new ObjectNotFoundException();
                }

            }

        }

        public void ApplyReferencePropertyChanges(
            IEntityWithRelationships newEntity,
            IEntityWithRelationships oldEntity)
        {
            foreach (var relatedEnd in oldEntity.RelationshipManager.GetAllRelatedEnds())
            {
                var oldRef = relatedEnd as EntityReference;
                if (oldRef != null)
                {
                    // this related end is a reference not a collection
                    var newRef = newEntity.RelationshipManager.GetRelatedEnd(oldRef.RelationshipName, oldRef.TargetRoleName) as EntityReference;
                    oldRef.EntityKey = newRef.EntityKey;
                }
            }
        }


        public void Delete<TEntity>(TEntity entity)
                    where TEntity : EntityObject
        {
            if (entity.EntityState != EntityState.Detached)
                this.Detach(entity);

            if (entity.EntityKey != null)
            {
                var onlyEntity = default(object);
                if (this.TryGetObjectByKey(entity.EntityKey, out onlyEntity))
                {
                    //this.Refresh(RefreshMode.StoreWins, entity);
                    this.DeleteObject(onlyEntity);
                    this.SaveChanges(true);
                }
            }
            else
            {
                this.Attach<TEntity>(entity);
                this.Refresh(RefreshMode.StoreWins, entity);
                this.DeleteObject(entity);
                this.SaveChanges(true);
            }
        }

        /// <summary>
        /// Goes through an ObjectStateEntry and sets every property other
        /// than the key properties to be modified.
        /// </summary>
        /// <param name="entry"></param>
        static void SetEntryModified(ObjectContext context, object item)
        {
            ObjectStateEntry entry = context.ObjectStateManager.GetObjectStateEntry(item);
            // One way of doing it, almost certainly others...
            for (int i = 0; i < entry.CurrentValues.FieldCount; i++)
            {
                bool isKey = false;

                string name = entry.CurrentValues.GetName(i);

                foreach (var keyPair in entry.EntityKey.EntityKeyValues)
                {
                    if (string.Compare(name, keyPair.Key, true) == 0)
                    {
                        isKey = true;
                        break;
                    }
                }
                if (!isKey)
                {
                    entry.SetModifiedProperty(name);
                }
            }
        }
    }
}
