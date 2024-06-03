namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddAppSettings(this WebApplicationBuilder builder)
		{
			builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);

			return builder;
		}
	}
}