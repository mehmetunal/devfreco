﻿using Dev.Core.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dev.Framework.Helper.ResponseRewind
{
    public class ExceptionHelper : BaseExceptionHelper, IExtensionsHelper
    {
        private HttpContext _context;
        public ExceptionHelper(HttpContext context) : base(context)
        {
            _context = context;
        }
        public async Task Bind(Stream body, Response<object> response, Exception ex)
        {
            response.SystemError = ex.Message;
            response.IsError = true;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await base.Bind(body, response);
        }
    }
}
