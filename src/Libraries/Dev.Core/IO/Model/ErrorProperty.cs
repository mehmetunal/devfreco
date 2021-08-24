﻿using System.Collections.Generic;

namespace Dev.Core.IO.Model
{
    public class ErrorProperty
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> FileExists { get; set; }
    }
}
