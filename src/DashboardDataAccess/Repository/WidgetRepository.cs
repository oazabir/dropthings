namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WidgetRepository : Dropthings.DataAccess.Repository.IWidgetRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public WidgetRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public List<Widget> GetAllWidgets(Enumerations.WidgetTypeEnum widgetType)
        {
            return _database.GetList<Widget, Enumerations.WidgetTypeEnum>(DropthingsDataContext.SubsystemEnum.Widget, widgetType, LinqQueries.CompiledQuery_GetAllWidgets);
        }

        public List<Widget> GetDefaultWidgetsByRole(string userName, Enumerations.WidgetTypeEnum widgetType, bool isDefault)
        {
            return _database.GetList<Widget, string, Enumerations.WidgetTypeEnum, bool>(DropthingsDataContext.SubsystemEnum.Widget, userName, widgetType, isDefault, LinqQueries.CompiledQuery_GetDefaultWidgetsByRole);
        }

        public Widget GetWidgetById(int id)
        {
            return _database.GetSingle<Widget, int>(DropthingsDataContext.SubsystemEnum.Widget, id, LinqQueries.CompiledQuery_GetWidgetById);
        }

        public List<Widget> GetWidgetByIsDefault(bool isDefault)
        {
            return _database.GetList<Widget, bool>(DropthingsDataContext.SubsystemEnum.Widget, isDefault, LinqQueries.CompiledQuery_GetWidgetByIsDefault);
        }

        public List<Widget> GetWidgetsByRole(string userName, Enumerations.WidgetTypeEnum widgetType)
        {
            return _database.GetList<Widget, string, Enumerations.WidgetTypeEnum>(DropthingsDataContext.SubsystemEnum.Widget, userName, widgetType, LinqQueries.CompiledQuery_GetWidgetsByRole);
        }

        #endregion Methods
    }
}