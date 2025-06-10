using ComputerStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Infrastructure.Services {
    public interface ITokenService {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
