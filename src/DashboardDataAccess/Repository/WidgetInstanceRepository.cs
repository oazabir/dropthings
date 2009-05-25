namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WidgetInstanceRepository : Dropthings.DataAccess.Repository.IWidgetInstanceRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public WidgetInstanceRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, id);
        }

        public void Delete(WidgetInstance page)
        {
            _database.Delete<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, page);
        }

        public WidgetInstance GetWidgetInstanceById(int id)
        {
            return _database.GetSingle<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, id, LinqQueries.CompiledQuery_GetWidgetInstanceById);
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneAfterPosition(int widgetZoneId, int position)
        {
            return _database.GetList<WidgetInstance, LinqQueries.WidgetZonePosition>(DropthingsDataContext.SubsystemEnum.WidgetInstance, new LinqQueries.WidgetZonePosition { Position = position, WidgetZoneId = widgetZoneId }, LinqQueries.CompiledQuery_GetWidgetInstanceOnWidgetZoneAfterPosition);
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneFromPosition(int widgetZoneId, int position)
        {
            return _database.GetList<WidgetInstance, LinqQueries.WidgetZonePosition>(DropthingsDataContext.SubsystemEnum.WidgetInstance, new LinqQueries.WidgetZonePosition { Position = position, WidgetZoneId = widgetZoneId }, LinqQueries.CompiledQuery_GetWidgetInstanceOnWidgetZoneFromPosition);
        }

        public string GetWidgetInstanceOwnerName(int widgetInstanceId)
        {
            return _database.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceOwnerName);
        }

        public List<WidgetInstance> GetWidgetInstancesByPageId(int pageId)
        {
            return _database.GetList<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, pageId, LinqQueries.CompiledQuery_GetWidgetInstancesByPageId);
        }

        public List<int> GetWidgetInstancesByRole(int widgetInstanceId, Guid roleId)
        {
            return _database.GetList<int, int, Guid>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstanceId, roleId, LinqQueries.CompiledQuery_GetWidgetInstancesByRole);
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetAndRole(int widgetId, Guid roleId)
        {
            return _database.GetList<WidgetInstance, int, Guid>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetId, roleId, LinqQueries.CompiledQuery_GetAllWidgetInstancesByWidgetAndRole);
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetZoneId(int widgetZoneId)
        {
            return _database.GetList<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetZoneIdWithWidget(int widgetZoneId)
        {
            return _database.GetList<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneIdWithWidget, LinqQueries.WidgetInstance_Options_With_Widget);
        }

        public WidgetInstance Insert(Action<WidgetInstance> populate)
        {
            return _database.Insert<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, populate);
        }

        public void InsertList(List<Widget> widgets, Converter<Widget, WidgetInstance> populate)
        {
            _database.InsertList<WidgetInstance, Widget>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgets.AsEnumerable<Widget>(), populate);
        }

        public void Update(WidgetInstance page, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate)
        {
            _database.UpdateObject<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, page, detach, postAttachUpdate);
        }

        public void UpdateList(List<WidgetInstance> widgetInstances, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate)
        {
            _database.UpdateList<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstances, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}