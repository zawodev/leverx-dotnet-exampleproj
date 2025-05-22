using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Product {
        // normal properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

        // navigation properties
        [JsonIgnore]
        public Category? Category { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
