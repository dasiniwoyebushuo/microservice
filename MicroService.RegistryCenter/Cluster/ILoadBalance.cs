using System.Collections.Generic;

namespace MicroService.RegistryCenter.Cluster
{
    public interface ILoadBalance
    {
        string SelectUrl(IList<string> urls);
    }
}
