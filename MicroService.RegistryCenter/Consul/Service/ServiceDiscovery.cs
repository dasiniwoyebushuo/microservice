using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consul;
using MicroService.RegistryCenter.IService;
using MicroService.RegistryCenter.ModuleOptions.Config;
using Microsoft.Extensions.Configuration;

namespace MicroService.RegistryCenter.Consul.Service
{
    /// <summary>
    /// consul服务发现实现
    /// </summary>
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly IConfiguration _configuration;

        public ServiceDiscovery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取指定服务名称的URL地址
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务对应的URL地址列表</returns>
        public async Task<IList<string>> Discovery(string serviceName)
        {
            ServiceDiscoveryConfig serviceDiscoveryConfig = _configuration.GetSection("ConsulDiscovery").Get<ServiceDiscoveryConfig>();

            // 1、创建consul客户端连接
            var consulClient = new ConsulClient(configuration =>
            {
                //1.1 建立客户端和服务端连接
                configuration.Address = new Uri(serviceDiscoveryConfig.RegistryAddress);
            });

            // 2、consul查询服务,根据具体的服务名称查询
            var queryResult = await consulClient.Catalog.Service(serviceName);

            // 3、将服务进行拼接
            var list = new List<string>();
            foreach (var service in queryResult.Response)
            {
                list.Add(service.ServiceAddress + ":" + service.ServicePort);
            }
            return list;
        }
    }
}
