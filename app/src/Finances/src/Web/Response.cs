using Finances.Exceptions;
using Finances.Web.Serialization;
using Newtonsoft.Json;

namespace Finances.Web {
    [JsonConverter(typeof(ResponseSerializer))]
    public class Response {
        public ApplicationError Error { get; }

        public Response() {
        }

        public Response(ApplicationError error) {
            Error = error;
        }
    }
}