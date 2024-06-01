namespace Catalog.Domain.Aggregates.Product
{
	public class Product : AggregateRoot<Guid>
	{
		private List<string> _categories;

		public string Name { get; private set; }
		public string Description { get; private set; }
		public string ImageFile { get; private set; }
		public decimal Price { get; private set; }
		public IReadOnlyCollection<string> Categories => _categories.ToArray();

		private Product()
		{
			Name = string.Empty;
			Description = string.Empty;
			ImageFile = string.Empty;
			_categories = new List<string>();
		}

		public Product(Guid id, string name, string[] categories, string description, string imageFile, decimal price)
		{
			Id = id;
			Name = name;
			Description = description;
			ImageFile = imageFile;
			Price = price;
			_categories = categories.ToList();
		}

		public static Product CreateNew(string name, string description, string imageFile, decimal price, string[] categories)
		{
			return new Product(
				id: Guid.NewGuid(),
				name: name,
				categories: categories,
				description: description,
				imageFile: imageFile,
				price: price);
		}

		public void Update(string name, string description, string imageFile, decimal price)
		{
			Name = name;
			Description = description;
			ImageFile = imageFile;
			Price = price;
		}

		public void AddCategories(string[] newCategories)
			=> _categories.AddRange(newCategories);
	}
}