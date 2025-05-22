using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Customer {
        // normal properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // navigation properties
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}
