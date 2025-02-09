namespace WebApplication11.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public static class SessionExtensions
    {
        public static void SetDictionary<TKey, TValue>(this ISession session, string key, Dictionary<TKey, TValue> value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? new Dictionary<TKey, TValue>() : JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(value);
        }
    }


}
