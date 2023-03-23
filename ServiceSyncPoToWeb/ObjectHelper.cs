using Newtonsoft.Json;

namespace ServiceSyncPoToWeb
{
   static class ObjectHelper
    {
        public static string DumpAsJson<T>(this T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return json;
        }
    }
}
