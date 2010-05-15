using System;
namespace Dropthings.Data.Repository
{
    public interface IColumnRepository : IDisposable
    {
        void Delete(Dropthings.Data.Column column);
        void Dispose();
        Dropthings.Data.Column GetColumnById(int id);
        Dropthings.Data.Column GetColumnByPageId_ColumnNo(int pageId, int columnNo);
        System.Collections.Generic.List<Dropthings.Data.Column> GetColumnsByPageId(int pageId);
        Dropthings.Data.Column Insert(Dropthings.Data.Column column);
        void Update(Dropthings.Data.Column column);
        void UpdateList(System.Collections.Generic.List<Dropthings.Data.Column> columns);
    }
}
