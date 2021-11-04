using MicroService.RegistryCenter.Consul;
using MicroService.RegistryCenter.ModuleOptions;
using MicroService.RegistryCenter.ModuleOptions.Config;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using MicroService.RegistryCenter.Cluster;
using MicroService.RegistryCenter.Consul.Service;
using MicroService.RegistryCenter.IService;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MicroService.RegistryCenter
{
    public class RegistryCenterModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection services = context.Services;

            //获取注册中心配置
            PreRegistryCenterOptions preRegistryCenterOptions = services.ExecutePreConfiguredActions<PreRegistryCenterOptions>();
            services.Configure<ServiceRegistryConfig>(context.Services.GetConfiguration().GetSection(preRegistryCenterOptions.ServiceRegistryConfigKey));

            //consul服务注册
            services.AddSingleton<IServiceRegistry, ServiceRegistry>();

            //consul服务发现
            services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            //负载均衡
            if (preRegistryCenterOptions.CustomizeLoadBalanceType != null)
            {
                //自定义负载均衡算法
                services.AddSingleton(typeof(ILoadBalance), preRegistryCenterOptions.CustomizeLoadBalanceType);
            }
            else
            {
                //默认随机负载均很算法
                services.AddSingleton<ILoadBalance, RandomLoadBalance>();
            }
            //负载均衡请求工具
            if (preRegistryCenterOptions.CustomizeLoadBalanceType != null)
            {
                //自定义负载均衡算法
                services.AddSingleton(typeof(ILoadBalance), preRegistryCenterOptions.CustomizeLoadBalanceType);
            }
            else
            {
                //默认随机负载均很算法
                services.AddSingleton<ILoadBalance, RandomLoadBalance>();
            }


        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            // 1、从IOC容器中获取Consul服务注册配置
            var serviceNode = app.ApplicationServices.GetRequiredService<IOptions<ServiceRegistryConfig>>().Value;

            // 2、获取应用程序生命周期
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 2.1 获取服务注册实例
            var serviceRegistry = app.ApplicationServices.GetRequiredService<IServiceRegistry>();

            // 3、获取本服务地址  可以换成其它的
            var features = app.Properties["server.Features"] as FeatureCollection;//启动文件获取
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var uri = new Uri(address);

            // 4、注册服务
            serviceNode.Id = Guid.NewGuid().ToString();
            serviceNode.Address = $"{uri.Scheme}://{uri.Host}";
            serviceNode.Port = uri.Port;
            serviceNode.HealthCheckAddress = $"{uri.Scheme}://{uri.Host}:{uri.Port}{serviceNode.HealthCheckAddress}";
            serviceRegistry.Register(serviceNode);

            // 5、服务器关闭时注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                serviceRegistry.DeRegister(serviceNode);
            });

        }
    }
}