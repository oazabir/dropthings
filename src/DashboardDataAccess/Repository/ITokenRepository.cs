namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface ITokenRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(Token page);

        Dropthings.DataAccess.Token GetTokenByUniqueId(Guid tokenId);

        Token Insert(Action<Token> populate);

        void Update(Token page, Action<Token> detach, Action<Token> postAttachUpdate);

        #endregion Methods
    }
}