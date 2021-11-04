using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MicroService.RegistryCenter.Cluster;
using MicroService.RegistryCenter.IService;
using Newtonsoft.Json;

namespace MicroService.RegistryCenter.Consul
{
    /// <summary>
    /// consul httpclient扩展
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

        /// <summary>
        /// Get方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// param name="ServiceSchme">服务名称:(http/https)</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="serviceLink">服务路径</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string serviceshcme, string serviceName, string serviceLink)
        {
            // 1、获取服务
            IList<ServiceUrl> serviceUrls = await _serviceDiscovery.Discovery(serviceName);

            // 2、负载均衡服务
            ServiceUrl serviceUrl = _loadBalance.Select(serviceUrls);

            // 3、建立请求
            System.Net.Http.HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl.Url + serviceLink);

            // 3.1json转换成对象
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new Exception($"{serviceName}服务调用错误");
            }
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
            httpClient.DefaultRequestHeaders.
            await httpClient.PostAsync(serviceUrl,);
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl.Url + serviceLink);

            // 3.1json转换成对象
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new Exception($"{ServiceName}服务调用错误");
            }
        }

        public Task<TResponse> GetAsync<TResponse>(string serviceName, string actionPath, string data, Dictionary<string, string> headers = null) where TResponse : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
