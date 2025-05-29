using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Address {
        // normal properties
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string? Line2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsBilling { get; set; }
        public int CustomerId { get; set; }

        // navigation properties
        [JsonIgnore]
        public Customer? Customer { get; set; }
    }
}
