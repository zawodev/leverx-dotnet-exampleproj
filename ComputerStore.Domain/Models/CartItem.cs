using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class CartItem {
        // normal properties
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // navigation properties
        [JsonIgnore]
        public Cart? Cart { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
