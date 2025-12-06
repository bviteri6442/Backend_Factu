using MongoDB.Bson;
using MongoDB.Driver;
using PuntoVenta.Domain.Entities;

try
{
	const string connectionString = "mongodb://localhost:27017";
	const string databaseName = "PuntoVentaDb";
	const string collectionName = "facturas";

	var client = new MongoClient(connectionString);
	var database = client.GetDatabase(databaseName);

	Console.WriteLine($"Conectado a MongoDB en {connectionString}");

	var rawCollection = database.GetCollection<BsonDocument>(collectionName);
	var rawDocs = rawCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
	Console.WriteLine($"Documentos en formato Bson: {rawDocs.Count}");

	foreach (var doc in rawDocs)
	{
		Console.WriteLine(doc.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
		{
			Indent = true
		}));
	}

	var facturaCollection = database.GetCollection<Factura>(collectionName);
	var facturas = facturaCollection.Find(Builders<Factura>.Filter.Empty).ToList();
	Console.WriteLine($"Documentos mapeados a Factura: {facturas.Count}");

	foreach (var factura in facturas)
	{
		Console.WriteLine($"Factura: {factura.NumeroFactura}, UsuarioId: {factura.UsuarioId}, ClienteId: {factura.ClienteId}, Total: {factura.TotalVenta}");
		Console.WriteLine($"  Fecha: {factura.FechaVenta:u}, Estado: {factura.Estado}, IVA: {factura.PorcentajeIVA}");
		Console.WriteLine($"  Detalles: {factura.Detalles.Count}");
	}
}
catch (Exception ex)
{
	Console.WriteLine("Error al consultar MongoDB: " + ex.Message);
	Console.WriteLine(ex);
}
