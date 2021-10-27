using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            PreGeneralOptions preGeneralOptions = services.ExecutePreConfiguredActions<PreGeneralOptions>();

            switch (preGeneralOptions.CacheType)
            {
                case CacheType.MemoryCache:
                    services.AddMemoryCache();
                    services.TryAddSingleton<ICache, MemoryCacheTools>();
                    break;
                case CacheType.Redis:
                    throw new Exception("Redis缓存暂不可用");
                case CacheType.Customize:
                    services.TryAddSingleton(typeof(ICache), preGeneralOptions.CustomizeCacheType);
                    break;
                default:
                    services.AddMemoryCache();
                    services.TryAddSingleton<ICache, MemoryCacheTools>();
                    break;

            }
        }
