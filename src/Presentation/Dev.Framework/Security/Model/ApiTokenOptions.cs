namespace Dev.Framework.Security.Model
{
    public class ApiTokenOptions
    {
        public string ApiName { get; set; }
        public string ApiVersion { get; set; }
        public string IdentityServerBaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        public string OidcSwaggerUIClientId { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string OidcApiName { get; set; }
        public string AdministrationRole { get; set; }
        public bool CorsAllowAnyOrigin { get; set; }
        public string[] CorsAllowOrigins { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
