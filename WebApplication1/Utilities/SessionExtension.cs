using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WebApplication1
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            Debug.WriteLine("Set Session ID " + session.Id);
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            Debug.WriteLine("Get Session ID " + session.Id);
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}