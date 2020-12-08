using System.Data;

namespace DatingApp.DatabaseInitializer
{
    public interface IDbConnectionProvider
    {
        string ConnectionString { get; }

        IDbConnection GetDbConnection();
    }
}