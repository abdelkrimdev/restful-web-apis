using Newtonsoft.Json;

namespace Shared.Tests.Utilities
{
    public static class  JSONUtils
    {
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
