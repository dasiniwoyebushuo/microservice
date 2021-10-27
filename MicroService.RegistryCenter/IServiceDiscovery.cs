using System.Collections.Generic;
using System.Threading.Tasks;
using MicroService.RegistryCenter.ModuleOptions.Config;

namespace MicroService.RegistryCenter
{
   /// <summary>
   /// 服务发现
   /// </summary>
   public interface IServiceDiscovery
   {
        /// <summary>
        /// 服务发现
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        Task<IList<ServiceUrl>> Discovery(string serviceName);
    }
}
