﻿using Dev.Dto.Npgsql.Identity.User;
using System;

namespace Dev.Dto.Npgsql.Identity.Token
{
    public class AccessTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; }
    }
}
