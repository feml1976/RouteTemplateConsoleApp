using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Core.Interfaces.RouteTemplateConsoleApp.Core.Interfaces.Data;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using Npgsql;
using Dapper;
using RouteTemplateConsoleApp.Core.Services;

namespace RouteTemplateConsoleApp.Infrastructure.Services
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<RouteRepository> _logger;

        public RouteRepository(IDbConnectionFactory connectionFactory, ILogger<RouteRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<bool> CreateTableIfNotExistsAsync(CancellationToken cancellationToken = default)
        {
            const string createTableSql = @"
                CREATE TABLE IF NOT EXISTS GetRouteWithStep (
                    Id BIGSERIAL PRIMARY KEY,
                    RouteId NUMERIC NOT NULL,
                    Name VARCHAR(255),
                    NumberOfLegs INTEGER,
                    DeparturePlace VARCHAR(255),
                    ArrivalPlace VARCHAR(255),
                    Code VARCHAR(50),
                    TimeStamp TIMESTAMP,
                    UserName VARCHAR(100),
                    Metros NUMERIC,
                    Segundos NUMERIC,
                    Steps JSONB,
                    State INTEGER DEFAULT 1,
                    CreateAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    UpdateAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            try
            {
                using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
                await connection.OpenAsync(cancellationToken);

                var result = await connection.ExecuteAsync(createTableSql);
                _logger.LogInformation("Tabla GetRouteWithStep verificada/creada exitosamente");

                return true;
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error de PostgreSQL al crear/verificar tabla");
                throw new DatabaseException("Error al crear/verificar tabla", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear/verificar tabla");
                throw new DatabaseException("Error inesperado al crear/verificar tabla", ex);
            }
        }

        public async Task<bool> InsertRouteAsync(GetRouteWithStep route, CancellationToken cancellationToken = default)
        {
            const string insertSql = @"
                INSERT INTO GetRouteWithStep 
                (RouteId, Name, NumberOfLegs, DeparturePlace, ArrivalPlace, Code, TimeStamp, UserName, Metros, Segundos, Steps, State, CreateAt, UpdateAt)
                VALUES 
                (@RouteId, @Name, @NumberOfLegs, @DeparturePlace, @ArrivalPlace, @Code, @TimeStamp, @UserName, @Metros, @Segundos, @Steps::jsonb, @State, @CreateAt, @UpdateAt)";

            try
            {
                _logger.LogDebug("Insertando ruta: {RouteId} - {Name}", route.RouteId, route.Name);

                using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
                await connection.OpenAsync(cancellationToken);

                var result = await connection.ExecuteAsync(insertSql, route);

                return result > 0;
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error de PostgreSQL al insertar ruta {RouteId}", route.RouteId);
                throw new DatabaseException($"Error al insertar ruta {route.RouteId}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar ruta {RouteId}", route.RouteId);
                throw new DatabaseException($"Error inesperado al insertar ruta {route.RouteId}", ex);
            }
        }


        /// <summary>
        /// Inserta o actualiza una lista de rutas en la base de datos.
        /// Si una ruta ya existe (basado en RouteId), se actualiza; de lo contrario, se inserta.
        /// </summary>
        /// <param name="routes">Lista de objetos <see cref="GetRouteWithStep"/> a insertar o actualizar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>
        /// <c>true</c> si al menos un registro fue afectado (insertado o actualizado); de lo contrario, <c>false</c>.
        /// </returns>
        /// <exception cref="DatabaseException">
        /// Se lanza cuando ocurre un error de base de datos (PostgreSQL u otro error inesperado) durante la operación.
        /// </exception>
        public async Task<bool> InsertRoutesAsync(List<GetRouteWithStep> routes, CancellationToken cancellationToken = default)
        {
            // SQL para insertar si no existe, o actualizar si existe.
            // Se asume que 'RouteId' es la clave primaria o una clave única para determinar la existencia.
            // La cláusula ON CONFLICT (RouteId) DO UPDATE es específica de PostgreSQL.
            const string upsertSql = @"
                        INSERT INTO GetRouteWithStep
                        (RouteId, Name, NumberOfLegs, DeparturePlace, ArrivalPlace, Code, TimeStamp, UserName, Metros, Segundos, Steps, State, CreateAt, UpdateAt)
                        VALUES
                        (@RouteId, @Name, @NumberOfLegs, @DeparturePlace, @ArrivalPlace, @Code, @TimeStamp, @UserName, @Metros, @Segundos, @Steps::jsonb, @State, @CreateAt, @UpdateAt)
                        ON CONFLICT (RouteId) DO UPDATE SET
                            Name = EXCLUDED.Name,
                            NumberOfLegs = EXCLUDED.NumberOfLegs,
                            DeparturePlace = EXCLUDED.DeparturePlace,
                            ArrivalPlace = EXCLUDED.ArrivalPlace,
                            Code = EXCLUDED.Code,
                            TimeStamp = EXCLUDED.TimeStamp,
                            UserName = EXCLUDED.UserName,
                            Metros = EXCLUDED.Metros,
                            Segundos = EXCLUDED.Segundos,
                            Steps = EXCLUDED.Steps::jsonb,
                            State = EXCLUDED.State,
                            UpdateAt = EXCLUDED.UpdateAt;
                    ";

            _logger.LogInformation("Intentando insertar o actualizar {Count} rutas en la base de datos", routes.Count);

            try
            {
                using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
                await connection.OpenAsync(cancellationToken);

                using var transaction = await connection.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await connection.ExecuteAsync(upsertSql, routes, transaction);

                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("Operación de inserción/actualización completada. {Count} registros afectados", result);
                    return result > 0;
                }
                catch (PostgresException ex)
                {
                    _logger.LogError(ex, "Error de PostgreSQL al insertar/actualizar rutas en lote");
                    throw new DatabaseException("Error al insertar o actualizar rutas en lote", ex);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Error durante la transacción al insertar/actualizar rutas en lote");
                    throw;
                }
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error de PostgreSQL al insertar/actualizar rutas en lote");
                throw new DatabaseException("Error al insertar o actualizar rutas en lote", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar/actualizar rutas en lote");
                throw new DatabaseException("Error inesperado al insertar o actualizar rutas en lote", ex);
            }
        }

        public async Task<bool> InsertRoutesAsyncOld(List<GetRouteWithStep> routes, CancellationToken cancellationToken = default)
        {
            const string insertSql = @"
                INSERT INTO GetRouteWithStep 
                (RouteId, Name, NumberOfLegs, DeparturePlace, ArrivalPlace, Code, TimeStamp, UserName, Metros, Segundos, Steps, State, CreateAt, UpdateAt)
                VALUES 
                (@RouteId, @Name, @NumberOfLegs, @DeparturePlace, @ArrivalPlace, @Code, @TimeStamp, @UserName, @Metros, @Segundos, @Steps::jsonb, @State, @CreateAt, @UpdateAt)";

            try
            {
                _logger.LogInformation("Insertando {Count} rutas en la base de datos", routes.Count);

                using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
                await connection.OpenAsync(cancellationToken);

                using var transaction = await connection.BeginTransactionAsync(cancellationToken);

                try
                {
                    var result = await connection.ExecuteAsync(insertSql, routes, transaction);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("Inserción completada. {Count} registros afectados", result);
                    return result > 0;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error de PostgreSQL al insertar rutas en lote");
                throw new DatabaseException("Error al insertar rutas en lote", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar rutas en lote");
                throw new DatabaseException("Error inesperado al insertar rutas en lote", ex);
            }
        }
    }
}
