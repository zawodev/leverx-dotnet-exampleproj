using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Customer {
        // normal properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public int UserId { get; set; }

        // navigation properties
        [JsonIgnore] 
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }

        [JsonIgnore]
        public ICollection<Cart>? Carts { get; set; }

        [JsonIgnore]
        public ICollection<ProductReview>? ProductReviews { get; set; }
    }
}
