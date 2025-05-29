using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class ProductReview {
        // normal properties
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // navigation properties
        [JsonIgnore]
        public Product? Product { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }
    }
}
