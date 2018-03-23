using System;
using System.Web;

namespace BNE.Web.LanHouse.Code
{
    /// <summary>
    /// Implementação do serviço de cache com cache HttpRuntime.Cache
    /// </summary>
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheID) as T;
            if (item == null)
            {
                item = getItemCallback();
                HttpContext.Current.Cache.Insert(cacheID, item); // expira em 20 mins
            }
            return item;
        }

        public bool Clear(string cacheID)
        {
            HttpRuntime.Cache.Remove(cacheID);
            return true;
        }
    }

    /// <summary>
    /// Interface do serviço de cache
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Retorna conteúdo cacheado ou preenche o cache com dados
        /// </summary>
        /// <typeparam name="T">tipo do conteúdo</typeparam>
        /// <param name="cacheID">nome da hash pela qual se identifica o conteúdo</param>
        /// <param name="getItemCallback">callback que retorna o conteúdo quando o cache está vazio</param>
        /// <returns></returns>
        T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;

        bool Clear(string cacheID);
    }
}