using System;
using MicroService.RegistryCenter.Cluster;
using MicroService.RegistryCenter.IService;

namespace MicroService.RegistryCenter.ModuleOptions
{
    public class PreRegistryCenterOptions
    {

        /// <summary>
        /// 注册中心配置键
        /// </summary>
        public string ServiceRegistryConfigKey { get; set; }

        public Type CustomizeLoadBalanceType { get; private set; }

        public Type CustomizeHttpClientType { get; private set; }

        public void SetCustomizeLoadBalanceType<T>() where T : class, ILoadBalance
        {
            CustomizeLoadBalanceType = typeof(T);
        }
        public void SetCustomizeHttpClientType<T>() where T : class, IHttpClient
        {
            CustomizeHttpClientType = typeof(T);
        }
    }
}
