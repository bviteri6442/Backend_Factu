using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using PuntoVenta.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Persistencia
{
    /// <summary>
    /// MongoDB database context with automatic index creation
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection") 
                                   ?? "mongodb://localhost:27017";
            var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName") 
                               ?? "PuntoVentaDb";
            
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            
            // Create indexes on initialization
            CreateIndexesAsync().GetAwaiter().GetResult();
        }

        // Collections (equivalent to DbSet in EF Core)
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");
        public IMongoCollection<Cliente> Clientes => _database.GetCollection<Cliente>("clientes");
        public IMongoCollection<Factura> Facturas => _database.GetCollection<Factura>("facturas");
        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("usuarios");
        public IMongoCollection<Rol> Roles => _database.GetCollection<Rol>("roles");
        public IMongoCollection<ErrorLog> ErrorLogs => _database.GetCollection<ErrorLog>("errorLogs");
        public IMongoCollection<IntentosLogin> IntentosLogin => _database.GetCollection<IntentosLogin>("intentosLogin");

        // Generic method to get any collection
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        // Transaction support for MongoDB
        public IClientSessionHandle StartSession()
        {
            return _client.StartSession();
        }

        /// <summary>
        /// Create unique indexes for data integrity
        /// </summary>
        private async Task CreateIndexesAsync()
        {
            try
            {
                // Products: Unique barcode
                var productBarcodeIndex = Builders<Product>.IndexKeys.Ascending(p => p.CodigoBarra);
                var productBarcodeOptions = new CreateIndexOptions { Unique = true, Name = "idx_codigoBarra_unique" };
                await Products.Indexes.CreateOneAsync(
                    new CreateIndexModel<Product>(productBarcodeIndex, productBarcodeOptions));

                // Products: Index on Activo for filtering
                var productActivoIndex = Builders<Product>.IndexKeys.Ascending(p => p.Activo);
                await Products.Indexes.CreateOneAsync(
                    new CreateIndexModel<Product>(productActivoIndex, new CreateIndexOptions { Name = "idx_activo" }));

                // Clientes: Unique document number
                var clienteDocIndex = Builders<Cliente>.IndexKeys.Ascending(c => c.Documento);
                var clienteDocOptions = new CreateIndexOptions { Unique = true, Name = "idx_documento_unique" };
                await Clientes.Indexes.CreateOneAsync(
                    new CreateIndexModel<Cliente>(clienteDocIndex, clienteDocOptions));

                // Usuarios: Unique email
                var usuarioEmailIndex = Builders<Usuario>.IndexKeys.Ascending(u => u.Correo);
                var usuarioEmailOptions = new CreateIndexOptions { Unique = true, Name = "idx_correo_unique" };
                await Usuarios.Indexes.CreateOneAsync(
                    new CreateIndexModel<Usuario>(usuarioEmailIndex, usuarioEmailOptions));

                // Usuarios: Unique cedula
                var usuarioCedulaIndex = Builders<Usuario>.IndexKeys.Ascending(u => u.Cedula);
                var usuarioCedulaOptions = new CreateIndexOptions { Unique = true, Name = "idx_cedula_unique" };
                await Usuarios.Indexes.CreateOneAsync(
                    new CreateIndexModel<Usuario>(usuarioCedulaIndex, usuarioCedulaOptions));

                // Facturas: Unique invoice number
                var facturaNumIndex = Builders<Factura>.IndexKeys.Ascending(f => f.NumeroFactura);
                var facturaNumOptions = new CreateIndexOptions { Unique = true, Name = "idx_numeroFactura_unique" };
                await Facturas.Indexes.CreateOneAsync(
                    new CreateIndexModel<Factura>(facturaNumIndex, facturaNumOptions));

                // Facturas: Compound index for date queries
                var facturaDateIndex = Builders<Factura>.IndexKeys
                    .Descending(f => f.FechaVenta)
                    .Ascending(f => f.Estado);
                await Facturas.Indexes.CreateOneAsync(
                    new CreateIndexModel<Factura>(facturaDateIndex, new CreateIndexOptions { Name = "idx_fechaVenta_estado" }));

                // IntentosLogin: Index on email for lockout queries
                var intentosEmailIndex = Builders<IntentosLogin>.IndexKeys.Ascending(i => i.Correo);
                await IntentosLogin.Indexes.CreateOneAsync(
                    new CreateIndexModel<IntentosLogin>(intentosEmailIndex, new CreateIndexOptions { Name = "idx_correo_intentos" }));

                // ErrorLogs: Index on date and severity for reporting
                var errorLogIndex = Builders<ErrorLog>.IndexKeys
                    .Descending(e => e.FechaError)
                    .Ascending(e => e.NivelSeveridad);
                await ErrorLogs.Indexes.CreateOneAsync(
                    new CreateIndexModel<ErrorLog>(errorLogIndex, new CreateIndexOptions { Name = "idx_fechaError_severidad" }));
            }
            catch (MongoCommandException ex) when (ex.CodeName == "IndexOptionsConflict")
            {
                // Indexes already exist, ignore
                Console.WriteLine("MongoDB indexes already created.");
            }
        }
    }
}