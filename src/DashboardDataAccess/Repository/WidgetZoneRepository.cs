namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WidgetZoneRepository : Dropthings.DataAccess.Repository.IWidgetZoneRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public WidgetZoneRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<WidgetZone, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, id);
        }

        public void Delete(WidgetZone page)
        {
            _database.Delete<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, page);
        }

        public WidgetZone GetWidgetZoneById(int widgetZoneId)
        {
            return _database.GetSingle<WidgetZone, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneById);
        }

        public WidgetZone GetWidgetZoneByPageId_ColumnNo(int pageId, int columnNo)
        {
            return _database.GetSingle<WidgetZone, int, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, pageId, columnNo, LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo);
        }

        public WidgetZone Insert(Action<WidgetZone> populate)
        {
            return _database.Insert<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, populate);
        }

        public void Update(WidgetZone page, Action<WidgetZone> detach, Action<WidgetZone> postAttachUpdate)
        {
            _database.UpdateObject<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, page, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}