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

        private void RemoveColumnInTabCacheEntries(Column column)
        {
            _cacheResolver.Remove(CacheKeys.TabKeys.ColumnsInTab(column.Tab.ID));
        }

        public void Delete(Column column)
        {
            RemoveColumnInTabCacheEntries(column);
            _database.Delete<Column>(column);
        }

        public Column GetColumnById(int id)
        {
            return _database.Query(
                    CompiledQueries.TabQueries.GetColumnById, id)
                .First();
        }

        public Column GetColumnByTabId_ColumnNo(int TabId, int columnNo)
        {
            return this.GetColumnsByTabId(TabId).Where(Tab => Tab.ColumnNo == columnNo)
                .First();
        }

        public List<Column> GetColumnsByTabId(int TabId)
        {
            return AspectF.Define
                .Cache<List<Column>>(_cacheResolver, CacheKeys.TabKeys.ColumnsInTab(TabId))
                .Return<List<Column>>(() =>
                    _database.Query(
                        CompiledQueries.TabQueries.GetColumnsByTabId, TabId)
                        .ToList());
        }

        public Column Insert(Column column)
        {
            var Tab = column.Tab;
            var zone = column.WidgetZone;

            column.Tab = null;
            column.WidgetZone = null;
            _database.Insert<Tab, WidgetZone, Column>(Tab, zone,
                (p, col) => col.Tab = Tab,
                (z, col) => col.WidgetZone = zone,
                column);

            column.Tab = Tab;
            column.WidgetZone = zone;
            RemoveColumnInTabCacheEntries(column);
            return column;

        }

        public void Update(Column column)
        {
            RemoveColumnInTabCacheEntries(column);
            _database.Update<Column>(column);
        }

        public void UpdateList(List<Column> columns)
        {
            columns.Each(column => RemoveColumnInTabCacheEntries(column));
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