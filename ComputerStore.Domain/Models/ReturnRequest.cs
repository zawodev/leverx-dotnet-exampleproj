using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class ReturnRequest {
        // normal properties
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime RequestedAt { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; }

        // navigation properties
        [JsonIgnore]
        public Order? Order { get; set; }

        [JsonIgnore]
        public ICollection<ReturnItem>? ReturnItems { get; set; }
    }
}
