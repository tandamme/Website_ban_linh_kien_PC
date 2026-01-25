using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Do_an_lap_trinh_c_.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}