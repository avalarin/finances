using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Finances.Web.Serialization {
    public class ResponseSerializer: JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var response = (Response)value;

            writer.WriteStartObject();

            writer.WritePropertyName("status");
            if (response.Error == null) {
                writer.WriteValue("Success");
            } else {
                writer.WriteValue(response.Error.Name);
                writer.WritePropertyName("message");
                writer.WriteValue(response.Error.Message);
            }

            var asPayloadResponse = response as PayloadResponse;
            if (asPayloadResponse != null) {
                foreach (var kvp in asPayloadResponse.Payload) {
                    writer.WritePropertyName(kvp.Key);
                    serializer.Serialize(writer, kvp.Value);
                }
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) {
            return typeof(Response).IsAssignableFrom(objectType);
        }
    }
}