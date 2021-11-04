using System.Collections.Generic;

namespace MicroService.RegistryCenter.Cluster
{
    public abstract class BaseLoadBalance : ILoadBalance
    {
        public string SelectUrl(IList<string> urls)
        {
            if (urls == null)
            {
                return null;
            }

            return urls.Count == 1 ? urls[0] : DoSelect(urls);
        }

        public abstract string DoSelect(IList<string> urls);
    }
}
