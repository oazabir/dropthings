namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ColumnRepository : Dropthings.DataAccess.Repository.IColumnRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public ColumnRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<Column, int>(DropthingsDataContext.SubsystemEnum.Column, id);
        }

        public void Delete(Column page)
        {
            _database.Delete<Column>(DropthingsDataContext.SubsystemEnum.Column, page);
        }

        public Column GetColumnById(int id)
        {
            return _database.GetSingle<Column, int>(DropthingsDataContext.SubsystemEnum.Column, id, LinqQueries.CompiledQuery_GetColumnById);
        }

        public Column GetColumnByPageId_ColumnNo(int pageId, int columnNo)
        {
            return _database.GetSingle<Column, int, int>(DropthingsDataContext.SubsystemEnum.Column, pageId, columnNo, LinqQueries.CompiledQuery_GetColumnByPageId_ColumnNo);
        }

        public List<Column> GetColumnsByPageId(int pageId)
        {
            return _database.GetList<Column, int>(DropthingsDataContext.SubsystemEnum.Column, pageId, LinqQueries.CompiledQuery_GetColumnsByPageId);
        }

        public Column Insert(Action<Column> populate)
        {
            return _database.Insert<Column>(DropthingsDataContext.SubsystemEnum.Column, populate);
        }

        public void Update(Column page, Action<Column> detach, Action<Column> postAttachUpdate)
        {
            _database.UpdateObject<Column>(DropthingsDataContext.SubsystemEnum.Column, page, detach, postAttachUpdate);
        }

        public void UpdateList(List<Column> columns, Action<Column> detach, Action<Column> postAttachUpdate)
        {
            _database.UpdateList<Column>(DropthingsDataContext.SubsystemEnum.Column, columns, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}