using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Category {
        // normal properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // navigation properties
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
