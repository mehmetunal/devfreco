﻿using Dev.Core.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dev.Framework.Helper.ResponseRewind
{
    public class NotFoundExceptionHelper : BaseExceptionHelper, IExtensionsHelper
    {
        private HttpContext _context;
        public NotFoundExceptionHelper(HttpContext context) : base(context)
        {
            _context = context;
        }
        public async Task Bind(Stream body, Response<object> response, Exception ex)
        {
            response.SystemError = ex.Message;
            response.IsError = true;
            response.StatusCode = StatusCodes.Status404NotFound;
            await base.Bind(body, response);
        }
    }
}
