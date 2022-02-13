﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Dev.Data
{
    public interface ILanguageModel
    {
        [Required]
        Guid LanguageId { get; set; }

        [Required]
        [StringLength(200)]
        string Content { get; set; }
    }
}
