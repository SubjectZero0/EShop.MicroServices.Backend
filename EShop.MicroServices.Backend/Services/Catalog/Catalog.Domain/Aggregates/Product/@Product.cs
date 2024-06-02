﻿using System.Text.Json.Serialization;

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
				description: description,
				imageFile: imageFile,
				price: price,
				categories: categories);
		}

		public void Update(string name, string description, string imageFile, decimal price)
		{
			Name = name;
			Description = description;
			ImageFile = imageFile;
			Price = price;
		}

		public void AddCategories(string[] newCategories)
			=> Categories.AddRange(newCategories);
	}
}