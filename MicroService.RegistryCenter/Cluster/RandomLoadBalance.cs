using System;
using System.Collections.Generic;

namespace MicroService.RegistryCenter.Cluster
{
    /// <summary>
    /// 随机负载均衡算法
    /// </summary>
    public class RandomLoadBalance : BaseLoadBalance
    {
        private readonly Random _random = new();
        public override string DoSelect(IList<string> urls)
        {
            // 1、获取随机数
            var index = _random.Next(urls.Count);

            // 2、选择一个服务进行连接
            return urls[index];
        }
    }
}
