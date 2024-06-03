var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres-eshop");

//-------------------------------------------------------------------

var catalogDb = postgres
	.WithEnvironment("POSTGRES_DB", "CatalogDb")
	.AddDatabase("CatalogDb", "CatalogDb");

builder.AddProject<Projects.Catalog_Api>("catalog-api")
	.WithReference(catalogDb)
	.WithReplicas(2);

//-------------------------------------------------------------------

var basketDb = postgres
	.WithEnvironment("POSTGRES_DB", "BasketDb")
	.AddDatabase("BasketDb", "BasketDb");

builder.AddProject<Projects.Basket_Api>("basket-api")
	.WithReference(basketDb);

builder.Build().Run();