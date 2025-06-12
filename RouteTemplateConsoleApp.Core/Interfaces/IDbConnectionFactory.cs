namespace RouteTemplateConsoleApp.Core.Interfaces
{
    // =====================================================
    // ARCHIVO: Interfaces/Data/IDbConnectionFactory.cs
    // =====================================================

    using System.Data;

    namespace RouteTemplateConsoleApp.Core.Interfaces.Data
    {
        public interface IDbConnectionFactory
        {
            IDbConnection CreateConnection();
            Task<IDbConnection> CreateConnectionAsync();
        }
    }
}
