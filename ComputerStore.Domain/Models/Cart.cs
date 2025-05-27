using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Cart {
        // normal properties
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }

        // navigation properties
        [JsonIgnore]
        public Customer? Customer { get; set; }

        [JsonIgnore]
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
