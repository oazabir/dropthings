namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IColumnRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(Column page);

        Dropthings.DataAccess.Column GetColumnById(int id);

        Dropthings.DataAccess.Column GetColumnByPageId_ColumnNo(int pageId, int columnNo);

        System.Collections.Generic.List<Dropthings.DataAccess.Column> GetColumnsByPageId(int pageId);

        Column Insert(Action<Column> populate);

        void Update(Column page, Action<Column> detach, Action<Column> postAttachUpdate);

        void UpdateList(List<Column> columns, Action<Column> detach, Action<Column> postAttachUpdate);

        #endregion Methods
    }
}