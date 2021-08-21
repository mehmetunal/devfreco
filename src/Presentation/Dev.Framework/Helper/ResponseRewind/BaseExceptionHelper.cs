﻿using Dev.Core.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Framework.Helper.ResponseRewind
{
    public class BaseExceptionHelper
    {
        internal byte[] GetBuffer(Response<object> json)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json));

        private HttpContext _context;
        public BaseExceptionHelper(HttpContext context)
        {
            _context = context;
        }
        internal async Task Bind(Stream body, Response<object> response)
        {
            _context.Response.ContentType = "application/json";
            _context.Response.ContentLength = response != null ? Encoding.UTF8.GetByteCount(JsonConvert.SerializeObject(response)) : 0;

            await using var output = new MemoryStream(GetBuffer(response));
            await output.CopyToAsync(body);
        }
    }
}
