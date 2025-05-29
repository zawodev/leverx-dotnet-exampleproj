using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class ReturnItem {
        // normal properties
        public int ReturnId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // navigation properties
        [JsonIgnore]
        public ReturnRequest? ReturnRequest { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
