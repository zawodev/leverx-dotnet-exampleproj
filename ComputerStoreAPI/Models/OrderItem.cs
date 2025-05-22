using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class OrderItem {
        // normal properties
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // navigation properties
        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
