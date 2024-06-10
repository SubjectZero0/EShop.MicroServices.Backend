namespace Basket.Api.Configurations
{
	public class Configurations
	{
		public class SqlConnectionConfiguration
		{
			public string? ConnectionString { get; init; }
		}
		
		public class RedisCacheConfiguration
		{
			public string Host { get; init; }
			public string Port { get; init; }
			public string ConnectionString { get; init; }
		}
	}
}