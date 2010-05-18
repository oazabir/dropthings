namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class ColumnRepository : Dropthings.Data.Repository.IColumnRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public ColumnRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemoveColumnInPageCacheEntries(Column column)
        {
            _cacheResolver.Remove(CacheKeys.PageKeys.ColumnsInPage(column.Page.ID));
        }

        public void Delete(Column column)
        {
            RemoveColumnInPageCacheEntries(column);
            _database.Delete<Column>(column);
        }

        public Column GetColumnById(int id)
        {
            return _database.Query(
                    CompiledQueries.PageQueries.GetColumnById, id)
                .First();
        }

        public Column GetColumnByPageId_ColumnNo(int pageId, int columnNo)
        {
            return this.GetColumnsByPageId(pageId).Where(page => page.ColumnNo == columnNo)
                .First();
        }

        public List<Column> GetColumnsByPageId(int pageId)
        {
            return AspectF.Define
                .Cache<List<Column>>(_cacheResolver, CacheKeys.PageKeys.ColumnsInPage(pageId))
                .Return<List<Column>>(() =>
                    _database.Query(
                        CompiledQueries.PageQueries.GetColumnsByPageId, pageId)
                        .ToList());
        }

        public Column Insert(Column column)
        {
            var page = column.Page;
            var zone = column.WidgetZone;

            column.Page = null;
            column.WidgetZone = null;
            _database.Insert<Page, WidgetZone, Column>(page, zone,
                (p, col) => col.Page = page,
                (z, col) => col.WidgetZone = zone,
                column);

            column.Page = page;
            column.WidgetZone = zone;
            RemoveColumnInPageCacheEntries(column);
            return column;

        }

        public void Update(Column column)
        {
            RemoveColumnInPageCacheEntries(column);
            _database.Update<Column>(column);
        }

        public void UpdateList(List<Column> columns)
        {
            columns.Each(column => RemoveColumnInPageCacheEntries(column));
            _database.UpdateList<Column>(columns);
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _database.Dispose();
        }

        #endregion
    }
}