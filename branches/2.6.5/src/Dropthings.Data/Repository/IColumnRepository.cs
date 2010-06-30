using System;
namespace Dropthings.Data.Repository
{
    public interface IColumnRepository : IDisposable
    {
        void Delete(Dropthings.Data.Column column);
        Dropthings.Data.Column GetColumnById(int id);
        Dropthings.Data.Column GetColumnByTabId_ColumnNo(int TabId, int columnNo);
        System.Collections.Generic.List<Dropthings.Data.Column> GetColumnsByTabId(int TabId);
        Dropthings.Data.Column Insert(Dropthings.Data.Column column);
        void Update(Dropthings.Data.Column column);
        void UpdateList(System.Collections.Generic.List<Dropthings.Data.Column> columns);
    }
}
