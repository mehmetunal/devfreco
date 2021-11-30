using Dev.Framework.Middleware;
using System;
using System.Threading.Tasks;

namespace Dev.Framework.HttpClientApi
{
    public partial class DevAPIClientHelper
    {
        private readonly APIAuthMiddelwareHelper _ApiClient;
        private readonly string _ServerUrl;
        public DevAPIClientHelper(string accessToken, string serverUrl)
        {
            _ApiClient = new APIAuthMiddelwareHelper(accessToken);
            _ServerUrl = serverUrl;
        }


        public async Task<object> Get(string path)
        {
            string requestUriString = string.Format("{0}{1}", _ServerUrl, path);

            using (_ApiClient.ApiMiddelwareClient)
            {
                var result = await _ApiClient.ApiMiddelwareClient.GetAsync(requestUriString);

                if (result.IsSuccessStatusCode)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();

                    return resultContent;
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }
        }
    }
}
