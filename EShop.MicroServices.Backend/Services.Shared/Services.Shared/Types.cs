using System.Text.Json.Serialization;

namespace Services.Shared
{
	[Serializable]
	public record ProductType
	{
		[JsonInclude]
		public string Name { get; init; }

		[JsonInclude]
		public string Description { get; init; }

		[JsonInclude]
		public string ImageFile { get; init; }

		[JsonInclude]
		public decimal Price { get; init; }

		[JsonInclude]
		public List<string> Categories { get; init; }
	}
}