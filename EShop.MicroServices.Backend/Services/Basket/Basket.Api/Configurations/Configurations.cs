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
			public string? ConnectionString { get; init; }
		}
	}
}