using System.Data;

namespace UserProfileRepository
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}
