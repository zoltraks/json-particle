using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BigBytes.JsonParticle
{
    /// <summary>
    /// Static implementation of Serialize and Deserialize methods using JsonConvert from Newtonsoft.Json package.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Json<T>
    {
        public static string Serialize(T o)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver,
            };
            Formatting formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(o, formatting, settings);
            return json;
        }

        public static T Deserialize(string json)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
            };
            T o = JsonConvert.DeserializeObject<T>(json, settings);
            return o;
        }
    }
}
