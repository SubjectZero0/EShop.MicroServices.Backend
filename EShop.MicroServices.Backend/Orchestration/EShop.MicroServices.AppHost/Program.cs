var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres(name: "postgres-eshop", port: 5438)
	.WithEnvironment("POSTGRES_USER", "postgres")
	.WithEnvironment("POSTGRES_PASSWORD", "postgres")
	.WithDataVolume();

var cache = builder.AddRedis("redis", port: 6379);
//-------------------------------------------------------------------

var catalogDb = postgres
	.WithEnvironment("POSTGRES_DB", "CatalogDb")
	.AddDatabase("CatalogDb", "CatalogDb");

// Add two instances of Catalog.Api
builder.AddProject<Projects.Catalog_Api>("catalog-api")
	.WithReference(catalogDb)
	.WithReplicas(2);
//-------------------------------------------------------------------

var basketDb = postgres
	.WithEnvironment("POSTGRES_DB", "BasketDb")
	.AddDatabase("BasketDb", "BasketDb");

// Add two instances of Basket.Api
builder.AddProject<Projects.Basket_Api>("basket-api")
	.WithReference(basketDb)
	.WithReference(cache)
	.WithReplicas(2);

builder.Build().Run();