var builder = DistributedApplication.CreateBuilder(args);

var catalogDb = builder.AddPostgres("postgres-eshop")
	.WithEnvironment("POSTGRES_DB", "CatalogDb")
	.AddDatabase("CatalogDb", "CatalogDb");

builder.AddProject<Projects.Catalog_Api>("catalog-api")
	.WithReference(catalogDb)
	.WithReplicas(2);

builder.Build().Run();