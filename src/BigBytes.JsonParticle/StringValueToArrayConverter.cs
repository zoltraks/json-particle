using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace BigBytes.JsonParticle
{
    /// <summary>
    /// 
    /// </summary>
    public class StringValueToArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new string[] { reader.Value as string };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                var t = new List<string>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndArray)
                    {
                        break;
                    }
                    if (reader.Value == null)
                    {
                        continue;
                    }
                    t.Add(reader.Value as string);
                }
                return t.ToArray();
            }
            else
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (null == value)
            {
                return;
            }
            if (value is string)
            {
                writer.WriteValue(value as string);
            }
            else if (value is string[])
            {
                if ((value as string[]).Length == 1)
                {
                    writer.WriteValue((value as string[])[0]);
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (string e in value as string[])
                    {
                        writer.WriteValue(e);
                    }
                    writer.WriteEndArray();
                }
            }
        }
    }
}