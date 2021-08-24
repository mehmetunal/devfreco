﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Dev.Core.IO.Model
{
    public class FileManagerParams
    {
        public string Name { get; set; }
        public string[] Names { get; set; }
        public string Path { get; set; }
        public string TargetPath { get; set; }
        public string NewName { get; set; }
        public object Date { get; set; }
        public IEnumerable<IFormFile> FileUpload { get; set; }
        public string[] ReplacedItemNames { get; set; }
    }
}
