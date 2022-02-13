using System.Collections.Generic;

namespace Dev.Core.Base
{
    public interface IResponse
    {
        List<string> Messages { get; set; }
        List<string> ValidationMessages { get; set; }
        int StatusCode { get; set; }
        bool Success { get; set; }
        bool IsError { get; set; }
    }
}