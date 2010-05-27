using System;
namespace Dropthings.Data.Repository
{
    public interface ITokenRepository : IDisposable
    {
        void Delete(Dropthings.Data.Token page);
        Dropthings.Data.Token GetTokenByUniqueId(Guid tokenId);
        Dropthings.Data.Token Insert(Dropthings.Data.Token token);
        void Update(Dropthings.Data.Token token);
    }
}
