using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.RegistryCenter.IService
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
        Task<IList<string>> Discovery(string serviceName);
    }
}
