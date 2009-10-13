namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;

    public class TokenRepository : Dropthings.DataAccess.Repository.ITokenRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICacheResolver _cacheResolver;

        #endregion Fields

        #region Constructors

        public TokenRepository(IDropthingsDataContext database, ICacheResolver cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<Token, int>(DropthingsDataContext.SubsystemEnum.Token, id);
        }

        public void Delete(Token page)
        {
            _database.Delete<Token>(DropthingsDataContext.SubsystemEnum.Token, page);
        }

        public Token GetTokenByUniqueId(Guid tokenId)
        {
            return _database.GetSingle<Token, Guid>(DropthingsDataContext.SubsystemEnum.Token, tokenId, LinqQueries.CompiledQuery_GetTokenByUniqueId);
        }

        public Token Insert(Action<Token> populate)
        {
            return _database.Insert<Token>(DropthingsDataContext.SubsystemEnum.Token, populate);
        }

        public void Update(Token page, Action<Token> detach, Action<Token> postAttachUpdate)
        {
            _database.UpdateObject<Token>(DropthingsDataContext.SubsystemEnum.Token, page, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}