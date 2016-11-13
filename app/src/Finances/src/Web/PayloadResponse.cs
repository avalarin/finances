using System.Collections.Generic;
using System.Reflection;
using Finances.Exceptions;
using Finances.Web.Serialization;
using Newtonsoft.Json;

namespace Finances.Web {
    [JsonConverter(typeof(ResponseSerializer))]
    public class PayloadResponse : Response {

        public LinkedList<KeyValuePair<string, object>> Payload { get; } = new LinkedList<KeyValuePair<string, object>>();

        public PayloadResponse(string name, object value) {
            Payload.AddFirst(new KeyValuePair<string, object>(name, value));
        }

        public PayloadResponse(IEnumerable<KeyValuePair<string, object>> kvps) {
            foreach(var kvp in kvps) {
                Payload.AddLast(kvp);
            }
        }

        // TODO perfomance question
        public PayloadResponse(object data) {
            var type = data.GetType();
            foreach(var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                Payload.AddLast(new KeyValuePair<string, object>(property.Name, property.GetValue(data)));
            }
        }

        public PayloadResponse(ApplicationError error):base(error) {
        }

    }
}