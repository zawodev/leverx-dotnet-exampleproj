using System.Text.Json.Serialization;

namespace ComputerStoreAPI.Models {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [JsonIgnore] public ICollection<UserRole>? UserRoles { get; set; }
        [JsonIgnore] public Customer? Customer { get; set; }

        // refresh tokens
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
