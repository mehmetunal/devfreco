﻿using Dev.Core.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Dev.Framework.HttpClientApi
{
    public class DevHttpClient : IDevHttpClient
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private decimal _languageId = 0;

        #endregion

        #region Ctor

        public DevHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            void Init()
            {
                _languageId = GetLang();
                if (_languageId != null)
                    _httpClient.DefaultRequestHeaders.Add("X-LanguageID", _languageId.ToString());

                var token = GetToken();
                if (token != null)
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var baseAddress = _configuration["HttpClientBaseAddress"];
                if (string.IsNullOrEmpty(baseAddress)) throw new ArgumentNullException("HttpClientBaseAddress");
                _httpClient.BaseAddress = new Uri(baseAddress);
            }

            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _configuration = configuration;
            Init();
        }

        #endregion


        #region Method

        /// <summary>
        /// Check whether the site is available
        /// </summary>
        /// <returns>The asynchronous task whose result determines that request is completed</returns>
        public virtual async Task PingAsync()
        {
            await _httpClient.GetStringAsync("/");
        }

        public virtual async Task<string> GetResource(string resourceKey)
        {
            var uri = $"{_languageId}/locale-string/{resourceKey}";

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<Task<string>>(responseString).Result;

            return response;
        }

        public virtual async Task<List<T>> GetAllAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(JsonConvert.DeserializeObject<Response<object>>(responseBody).Result.ToString())["Data"];
            if (jObject.Count() == 0)
                return await Task.FromResult(Activator.CreateInstance<List<T>>());

            var result = JsonConvert.DeserializeObject<List<T>>(jObject.ToString());

            return result;
        }

        public virtual async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(JsonConvert.DeserializeObject<Response<object>>(responseBody).Result.ToString())["Data"];
            if (jObject.Count() == 0)
                return Activator.CreateInstance<T>();

            var result = JsonConvert.DeserializeObject<T>(jObject.ToString());

            return result;
        }

        public virtual async Task<HttpResponseMessage> GetClient(string url, Dictionary<string, string> qParametre = null)
        {
            try
            {
                //if (qParametre != null)
                //{
                //    _queryParametre = qParametre.Union(_queryParametre.Where(k => !qParametre.ContainsKey(k.Key))).ToDictionary(k => k.Key, v => v.Value);
                //}
                var requestUri = QueryHelpers.AddQueryString(url, qParametre);
                return await _httpClient.GetAsync(requestUri);
            }
            catch
            {
                return null;
            }
        }

        private string GetToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("Authorization");

            return token;
        }

        public decimal GetLang()
        {
            var languageId = _httpContextAccessor.HttpContext.Request.Headers["X-LanguageID"].FirstOrDefault();

            if (string.IsNullOrEmpty(languageId))
                throw new ArgumentNullException("X-LanguageID");

            return decimal.Parse(languageId);
        }

        #endregion
    }
}
