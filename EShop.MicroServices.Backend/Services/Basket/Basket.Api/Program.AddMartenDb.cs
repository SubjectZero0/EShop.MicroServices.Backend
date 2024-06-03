using Marten;
using System.Text.Json;
using static Basket.Api.Configurations.Configurations;

namespace Basket.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddMartenDb(this WebApplicationBuilder builder)
		{
			var sqlConfiguration = builder
				.Configuration
				.GetSection(nameof(SqlConnectionConfiguration))
				.Get<SqlConnectionConfiguration>()
				?? throw new ArgumentNullException(nameof(SqlConnectionConfiguration));

			var connectionString = sqlConfiguration.ConnectionString ?? throw new ArgumentNullException("ConnectionString");

			builder.Services.AddMarten(cfg =>
			{
				cfg.Connection(connectionString);
				cfg.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.CreateOrUpdate;

				cfg.UseSystemTextJsonForSerialization(
					options: new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true,
						MaxDepth = 3,
						IncludeFields = true
					});

				//cfg.Schema.For<Product>().Identity(product => product.Id);
				////TODO: find a way to index categories
				//cfg.Schema.For<Product>().Duplicate(product => product.Name, "varchar(255)");
			})
			.UseLightweightSessions();

			return builder;
		}
	}
}