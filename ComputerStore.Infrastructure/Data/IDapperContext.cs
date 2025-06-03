using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ComputerStore.Infrastructure.Data {
    public interface IDapperContext {
        IDbConnection CreateConnection();
    }
}
