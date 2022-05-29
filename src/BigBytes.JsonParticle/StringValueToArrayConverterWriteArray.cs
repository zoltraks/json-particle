using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace BigBytes.JsonParticle
{
    public class StringValueToArrayConverterWriteArray : JsonConverter
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
                writer.WriteStartArray();
                writer.WriteValue(value as string);
                writer.WriteEndArray();
            }
            else if (value is string[])
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