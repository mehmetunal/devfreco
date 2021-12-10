using System;
using System.IO;
using Dev.Core.Base;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dev.Framework.Helper.ResponseRewind
{
    public class UnauthorizedAccessExceptionHelper : BaseResponseHelper, IExtensionsHelper
    {
        private HttpContext _context;

        public UnauthorizedAccessExceptionHelper(HttpContext context) : base(context)
        {
            _context = context;
        }

        public async Task Bind(Stream body, Response<object> response, Exception ex)
        {
            response.SystemError = ex.Message;
            response.IsError = true;
            response.StatusCode = StatusCodes.Status401Unauthorized;
            await base.Bind(body, response);
        }
    }
}
