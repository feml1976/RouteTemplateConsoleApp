namespace RouteTemplateConsoleApp.Core.Interfaces
{
    public interface IRouteProcessingService
    {
        Task ProcessRoutesAsync(CancellationToken cancellationToken = default);
    }
}
