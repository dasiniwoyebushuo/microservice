using MicroService.RegistryCenter.Cluster;
using MicroService.RegistryCenter.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.RegistryCenter.Consul
{
    /// <summary>
    /// consul httpClient扩展
    /// </summary>
    public class HttpClientTools : IHttpClient
    {
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILoadBalance _loadBalance;
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientTools(IServiceDiscovery serviceDiscovery,
                                    ILoadBalance loadBalance,
                                    IHttpClientFactory httpClientFactory)
        {
            _serviceDiscovery = serviceDiscovery;
            _loadBalance = loadBalance;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string serviceName, string actionPath, TRequest data, Dictionary<string, string> headers = null) where TRequest : class, new() where TResponse : class, new()
        {
            // 1、获取服务
            IList<string> serviceUrls = await _serviceDiscovery.Discovery(serviceName);

            // 2、负载均衡服务
            string serviceUrl = _loadBalance.SelectUrl(serviceUrls);

            // 3、建立请求
            HttpClient httpClient = _httpClientFactory.CreateClient();
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, headers[key]);
                }
            }
            HttpResponseMessage response = await httpClient.PostAsync(serviceUrl + actionPath, new StringContent(JsonConvert.SerializeObject(data)));

            // 3.1json转换成对象
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{serviceName}服务调用错误");
            }
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(json);

        }

        public async Task<TResponse> GetAsync<TResponse>(string serviceName, string actionPath, string data, Dictionary<string, string> headers = null) where TResponse : class, new()
        {
            IList<string> serviceUrls = await _serviceDiscovery.Discovery(serviceName);

            // 2、负载均衡服务
            string serviceUrl = _loadBalance.SelectUrl(serviceUrls);

            // 3、建立请求
            HttpClient httpClient = _httpClientFactory.CreateClient();
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, headers[key]);
                }
            }
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl + actionPath + "?" + data);

            // 3.1json转换成对象
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{serviceName}服务调用错误");
            }
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(json);
        }

    }
}
