using MicroService.RegistryCenter.ModuleOptions.Config;

namespace MicroService.RegistryCenter
{
    /// <summary>
    /// 服务注册
    /// </summary>
   public interface IServiceRegistry
   {
        /// <summary>
        /// 注册服务
        /// </summary>
        void Register(ServiceRegistryConfig serviceNode);

        /// <summary>
        /// 撤销服务
        /// </summary>
        void DeRegister(ServiceRegistryConfig serviceNode);
    }
}
