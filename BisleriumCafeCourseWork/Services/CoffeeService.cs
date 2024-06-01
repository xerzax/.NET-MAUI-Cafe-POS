using BisleriumCafeCourseWork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BisleriumCafeCourseWork.Services
{
    public class CoffeeService :GenericService<CoffeeType>
    {
		// Paths for app data and product files
		public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
        public static string appProductsFilePath = UtilityService.GetAppCoffeeFilePath();

		// Retrieve a CoffeeType by its ID
		public static CoffeeType GetById(Guid Id)
        {
            var coffee = GetAll(appProductsFilePath).FirstOrDefault(x => x.Id == Id);

            if (coffee == null)
            {
                throw new Exception("Product not found.");
            }

            return coffee;
        }
		// Delete a CoffeeType by its ID
		public static List<CoffeeType> Delete(Guid id)
		{
			var coffees = GetAll(appProductsFilePath);
			var coffeeToDelete = coffees.FirstOrDefault(x => x.Id == id);

			if (coffeeToDelete == null)
			{
				throw new Exception("Coffee type not found.");
			}

			coffees.Remove(coffeeToDelete);

			SaveAll(coffees, appDataDirectoryPath, appProductsFilePath);

			return coffees;
		}

		// Create a new CoffeeType
		public static List<CoffeeType> Create(Guid userId, string type, double price)
        {
			if (type == "")
			{
				throw new Exception("Please insert correct and valid input for each of the fields.");
			}

			if (price <= 0)
			{
				throw new Exception("Add a positive integer value to add a product item.");
			}

			type = type.Trim().ToLower();

			var coffeeList = GetAll(appProductsFilePath);

			// Check for existing product with the same type
			var productExists = coffeeList.Any(x => x.Type.ToLower() == type);

			if (productExists)
			{
				throw new Exception("Product with the same already exists, Please add any other title.");
			}

			// Create new coffee type
			var coffee = new CoffeeType()
			{
				Id = Guid.NewGuid(), // Assign a unique ID
				Type = type,
				Price = price,
				CreatedBy = userId
			};

			coffeeList.Add(coffee);

			SaveAll(coffeeList, appDataDirectoryPath, appProductsFilePath);

			return coffeeList;
		}

		// Edit an existing CoffeeType
		public static List<CoffeeType> Edit(Guid id, string type, double price)
		{
			List<CoffeeType> coffeeList = GetAll(appProductsFilePath);
			var coffeeToUpdate = coffeeList.FirstOrDefault(x => x.Id == id);

			if (coffeeToUpdate == null)
			{
				throw new Exception("Coffee not found.");
			}

			if (type == "")
			{
				throw new Exception("Please insert correct and valid input for each of the fields.");
			}

			if (price <= 0)
			{
				throw new Exception("Add a positive integer value to add a product item.");
			}
			coffeeToUpdate.Type = type.Trim();
			coffeeToUpdate.Price = price;

			SaveAll(coffeeList, appDataDirectoryPath, appProductsFilePath);

			return coffeeList;
		}

		// Merge function for Merge Sort
		public static List<CoffeeType> Merge(List<CoffeeType> left, List<CoffeeType> right)
		{
			var result = new List<CoffeeType>();

			while (left.Count > 0 || right.Count > 0)
			{
				if (left.Count > 0 && right.Count > 0)
				{
					if (left.First().Price <= right.First().Price)
					{
						result.Add(left.First());
						left.Remove(left.First());
					}
					else
					{
						result.Add(right.First());
						right.Remove(right.First());
					}
				}
				else if (left.Count > 0)
				{
					result.Add(left.First());
					left.Remove(left.First());
				}
				else if (right.Count > 0)
				{
					result.Add(right.First());

					right.Remove(right.First());
				}
			}
			return result;
		}
		public static List<CoffeeType> MergeSort(List<CoffeeType> unsorted)
		{
			if (unsorted.Count <= 1)
				return unsorted;

			var left = new List<CoffeeType>();
			var right = new List<CoffeeType>();

			int middle = unsorted.Count / 2;

			for (int i = 0; i < middle; i++)
			{
				left.Add(unsorted[i]);
			}

			for (int i = middle; i < unsorted.Count; i++)
			{
				right.Add(unsorted[i]);
			}

			left = MergeSort(left);
			right = MergeSort(right);
			return Merge(left, right);
		}
	}
}
