using System;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;

namespace NetKeystore.Converters
{
    internal class ByteArrayHexConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var hex = new StringBuilder();
            foreach (byte b in (byte[])value)
                hex.Append(b.ToString("x2"));
            writer.WriteValue(hex.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string hex = (string)reader.Value;
            if (hex.Length % 2 != 0)
                hex = "0" + hex;
            byte[] ret = new byte[hex.Length / 2];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            return ret;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(byte[]).IsAssignableFrom(objectType);
        }
    }
}