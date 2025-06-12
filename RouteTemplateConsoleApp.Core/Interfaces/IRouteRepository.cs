using RouteTemplateConsoleApp.Core.Models;

namespace RouteTemplateConsoleApp.Core.Interfaces
{
    public interface IRouteRepository
    {
        Task<bool> InsertRouteAsync(GetRouteWithStep route, CancellationToken cancellationToken = default);
        Task<bool> InsertRoutesAsync(List<GetRouteWithStep> routes, CancellationToken cancellationToken = default);
        Task<bool> CreateTableIfNotExistsAsync(CancellationToken cancellationToken = default);
    }
}
