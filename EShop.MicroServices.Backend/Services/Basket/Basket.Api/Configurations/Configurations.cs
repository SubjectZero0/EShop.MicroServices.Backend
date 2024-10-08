namespace Basket.Api.Configurations
{
	public class SqlConnectionConfiguration
	{
		public string? ConnectionString { get; init; }
	}

	public class RedisCacheConfiguration
	{
		public int DefaultDb { get; set; }
		public bool AllowAdmin { get; set; }
		public string ConnectionString { get; set; }
	}
}