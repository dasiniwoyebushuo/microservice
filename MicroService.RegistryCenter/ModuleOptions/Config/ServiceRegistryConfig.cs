namespace MicroService.RegistryCenter.ModuleOptions.Config
{
    /// <summary>
    /// 注册中心配置
    /// </summary>
    public class ServiceRegistryConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务标签(版本)
        /// </summary>
        public string[] Tags { set; get; }

        /// <summary>
        /// 服务地址(可以选填 === 默认加载启动路径)
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 服务端口号(可以选填 === 默认加载启动路径端口)
        /// </summary>
        public int Port { set; get; }

        /// <summary>
        /// 服务注册地址
        /// </summary>
        public string RegistryAddress { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string HealthCheckAddress { get; set; }
    }
}
