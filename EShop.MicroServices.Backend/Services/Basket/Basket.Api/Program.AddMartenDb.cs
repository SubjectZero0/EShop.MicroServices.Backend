using Marten;
using System.Text.Json;
using Basket.Domain.Aggregates.ShoppingCarts;
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

			var connectionString = sqlConfiguration.ConnectionString ?? throw new ArgumentNullException(nameof(sqlConfiguration.ConnectionString));

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