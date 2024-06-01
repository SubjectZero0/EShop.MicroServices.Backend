using System.Text.Json.Serialization;

namespace Catalog.Domain.Aggregates.Product
{
	public class Product : AggregateRoot<Guid>
	{
		[JsonInclude]
		public string Name { get; private set; }

		[JsonInclude]
		public string Description { get; private set; }

		[JsonInclude]
		public string ImageFile { get; private set; }

		[JsonInclude]
		public decimal Price { get; private set; }
		public IReadOnlyCollection<string> Categories => _categories.ToArray();

		[JsonInclude]
		public List<string> Categories { get; private set; }

		public Product()
		{ }

		public Product(Guid id, string name, string description, string imageFile, decimal price, string[] categories)
		{
			Id = id;
			Name = name;
			Description = description;
			ImageFile = imageFile;
			Price = price;
			Categories = categories.ToList();
		}

		public static Product CreateNew(string name, string description, string imageFile, decimal price, string[] categories)
		{
			return new Product(
				id: Guid.NewGuid(),
				name: name,
				categories: categories,
				description: description,
				imageFile: imageFile,
				price: price,
				categories: categories);
		}

		public void Update(string? name, string? description, string? imageFile, decimal? price)
		{
			Name = name ?? Name;
			Description = description ?? Description;
			ImageFile = imageFile ?? ImageFile;
			Price = price ?? Price;
		}

		public void AddCategories(string[] newCategories)
		{
			foreach (var newCategory in newCategories)
			{
				if (Categories.Contains(newCategory))
					continue;

				Categories.Add(newCategory);
			}
		}

		public void RemoveCategories(string[] categories)
			=> Categories = Categories.Except(categories).ToList();
	}
}