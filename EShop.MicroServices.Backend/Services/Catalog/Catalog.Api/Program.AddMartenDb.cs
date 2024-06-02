using Catalog.Domain.Aggregates.Product;
using Marten;
using static Catalog.Api.Configuration.Configuration;

namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddMartenDb(this WebApplicationBuilder builder)
		{
			var sqlCofinguration = builder
				.Configuration
				.GetSection(nameof(SqlConnectionConfiguration))
				.Get<SqlConnectionConfiguration>()
				?? throw new ArgumentNullException(nameof(SqlConnectionConfiguration));

			builder.Services.AddMarten(cfg =>
			{
				cfg.Connection(sqlCofinguration.ConnectionString);
				cfg.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.CreateOrUpdate;
				cfg.UseSystemTextJsonForSerialization();

				cfg.Schema.For<Product>().Identity(product => product.Id);
				//TODO: find a way to index categories
				cfg.Schema.For<Product>().Duplicate(product => product.Name, "varchar(255)");
			})
			.UseLightweightSessions();

			return builder;
		}
	}
}