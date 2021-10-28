using MicroService.RegistryCenter.ModuleOptions.Config;

namespace MicroService.RegistryCenter.ModuleOptions
{
    public class PreRegistryCenterOptions
    {
        /// <summary>
        /// 是否使用默认的Consul注册中心
        /// </summary>
        public bool UseDefaultConsulRegistryCenter { get; set; } = true;

        /// <summary>
        /// 注册中心配置
        /// </summary>
        public string ServiceRegistryConfigKey { get; set; }
    }
}
