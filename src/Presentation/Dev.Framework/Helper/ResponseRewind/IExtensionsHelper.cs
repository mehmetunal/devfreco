using System;
using System.IO;
using Dev.Core.Base;
using System.Threading.Tasks;

namespace Dev.Framework.Helper.ResponseRewind
{
    public interface IExtensionsHelper
    {
        Task Bind(Stream body, Response<object> response, Exception ex);
    }
}
