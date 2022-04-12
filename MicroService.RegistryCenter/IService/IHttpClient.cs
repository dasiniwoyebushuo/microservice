using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.RegistryCenter.IService
{
    public interface IHttpClient
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="actionPath">路径</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">追加头部信息</param>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <typeparam name="TResponse">返回类型</typeparam>
        /// <returns>返回数据对象</returns>
        Task<TResponse> PostAsync<TRequest, TResponse>(string serviceName, string actionPath, TRequest data, Dictionary<string, string> headers = null) where TRequest : class, new() where TResponse : class, new();


        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="actionPath">路径</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers"></param>
        /// <typeparam name="TResponse">返回类型</typeparam>
        /// <returns>返回数据对象</returns>
        Task<TResponse> GetAsync<TResponse>(string serviceName, string actionPath, string data, Dictionary<string, string> headers = null) where TResponse : class, new();
    }
}
