using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Order {
        // normal properties
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // navigation properties
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
