namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;

    public class TokenRepository : Dropthings.Data.Repository.ITokenRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public TokenRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(Token page)
        {
            _database.Delete<Token>(page);
        }

        public Token GetTokenByUniqueId(Guid tokenId)
        {
            return _database.Query<Guid, Token>(
                CompiledQueries.MiscQueries.GetTokenByUniqueId, tokenId)
                .First();
        }

        public Token Insert(Token token)
        {
            var user = token.aspnet_Users;
            token.aspnet_Users = null;
            _database.Insert<aspnet_User, Token>(user,
                (u, t) => t.aspnet_Users = u,
                token);
            token.aspnet_Users = user;
            return token;
        }

        public void Update(Token token)
        {
            _database.Update<Token>(token);
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