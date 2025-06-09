using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Infrastructure.Services {
    public interface IPasswordHasher {
        string Hash(string password);
        bool Verify(string hash, string password);
    }

    public class BCryptPasswordHasher : IPasswordHasher {
        public string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string hash, string password)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
