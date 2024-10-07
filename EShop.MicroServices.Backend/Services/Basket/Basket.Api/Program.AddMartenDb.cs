using Marten;
using System.Text.Json;
using Basket.Domain.Aggregates.ShoppingCarts;
using Basket.Api.Configurations;

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
				?? throw new Exception("nameof(SqlConnectionConfiguration) not found.");

			var connectionString = sqlConfiguration.ConnectionString ?? throw new Exception("nameof(sqlConfiguration.ConnectionString) is null");

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

				cfg.Schema.For<ShoppingCart>().Identity(cart => cart.Id);
				cfg.Schema.For<ShoppingCart>().Duplicate(cart => cart.UserName, pgType: "varchar(255)", notNull: true);

				cfg.Schema.For<ShoppingCartItem>().Identity(item => item.ProductId + Guid.NewGuid().ToString()).UseIdentityKey();
			})
			.UseLightweightSessions();

			return builder;
		}
	}
}