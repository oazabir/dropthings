namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class ColumnRepository : Dropthings.DataAccess.Repository.IColumnRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public ColumnRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemoveColumnInPageCacheEntries(Column column)
        {
            _cacheResolver.Remove(CacheSetup.CacheKeys.ColumnsInPage(column.PageId));
        }

        public void Delete(int id)
        {
            RemoveColumnInPageCacheEntries(this.GetColumnById(id));
            _database.DeleteByPK<Column, int>(DropthingsDataContext.SubsystemEnum.Column, id);
        }

        public void Delete(Column column)
        {
            RemoveColumnInPageCacheEntries(column);
            _database.Delete<Column>(DropthingsDataContext.SubsystemEnum.Column, column);
        }

        public Column GetColumnById(int id)
        {
            return _database.GetSingle<Column, int>(DropthingsDataContext.SubsystemEnum.Column, id, LinqQueries.CompiledQuery_GetColumnById);
        }

        public Column GetColumnByPageId_ColumnNo(int pageId, int columnNo)
        {
            return this.GetColumnsByPageId(pageId).Where(page => page.ColumnNo == columnNo).First();
        }

        public List<Column> GetColumnsByPageId(int pageId)
        {
            return AspectF.Define
                .Cache<List<Column>>(_cacheResolver, CacheSetup.CacheKeys.ColumnsInPage(pageId))
                .Return<List<Column>>(() =>
                    _database.GetList<Column, int>(DropthingsDataContext.SubsystemEnum.Column, pageId, LinqQueries.CompiledQuery_GetColumnsByPageId));
        }

        public Column Insert(Action<Column> populate)
        {
            var column = _database.Insert<Column>(DropthingsDataContext.SubsystemEnum.Column, populate);
            RemoveColumnInPageCacheEntries(column);
            return column;
        }

        public void Update(Column column, Action<Column> detach, Action<Column> postAttachUpdate)
        {
            RemoveColumnInPageCacheEntries(column);
            _database.UpdateObject<Column>(DropthingsDataContext.SubsystemEnum.Column, column, detach, postAttachUpdate);
        }

        public void UpdateList(List<Column> columns, Action<Column> detach, Action<Column> postAttachUpdate)
        {
            columns.Each(column => RemoveColumnInPageCacheEntries(column));
            _database.UpdateList<Column>(DropthingsDataContext.SubsystemEnum.Column, columns, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}