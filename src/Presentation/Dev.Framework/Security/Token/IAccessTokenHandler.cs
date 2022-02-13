using Dev.Core.IoC;
using Dev.Framework.Security.Model;

namespace Dev.Framework.Security.Token
{
    public interface IAccessTokenHandler : IService
    {
        /// <summary>
        /// Token Almak İçin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        AccessTokenDto CreateAccessToken(object user);

        object GetAccessToken();
    }
}
