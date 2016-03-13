using UserProfileRepository;

namespace UserProfileEntry
{
    public static class ConnectionHelper
    {
        public static IConnectionFactory GetConnection()
        {
            return new DbConnectionFactory("MyConString");
        }
    }
}