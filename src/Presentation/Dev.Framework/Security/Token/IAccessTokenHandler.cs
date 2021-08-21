using Dev.Core.IoC;
using Dev.Dto.Npgsql.Identity.Token;
using Dev.Dto.Npgsql.Identity.User;

namespace Dev.Framework.Security.Token
{
    public interface IAccessTokenHandler : IService
    {
        /// <summary>
        /// Token Almak İçin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        AccessTokenDto CreateAccessToken(UserDto user);

        UserDto GetAccessToken();
    }
}
