using System.Text.Json;
using System.Text.Json.Serialization;

namespace Seeding
{
    public static class JsonSeed
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public static T Load<T>(string path)
        {
            string data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, SerializerOptions);
        }
    }
}
