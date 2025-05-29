using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class Role {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public ICollection<UserRole>? UserRoles { get; set; }
    }
}
