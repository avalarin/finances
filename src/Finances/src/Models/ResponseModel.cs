using Newtonsoft.Json;

namespace Finances.Models {
    public abstract class ResponseModel<TStatus> {
        [JsonIgnore]
        public TStatus Status { get; }

        [JsonProperty(PropertyName = "status")]
        public string StatusString => Status.ToString();

        protected ResponseModel(TStatus status) {
            Status = status;
        }
    }
}