using BisleriumCafeCourseWork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Services
{
    public class AddOnService:GenericService<CoffeeAddIns>
    {
		// Paths for app data and product files
		public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
		public static string appProductsFilePath = UtilityService.GetAppCoffeeAddInsFilePath();

		// Retrieve a CoffeeAddIn by its ID
		public static CoffeeAddIns GetById(Guid Id)
		{
			var coffee = GetAll(appProductsFilePath).FirstOrDefault(x => x.Id == Id);

			if (coffee == null)
			{
				throw new Exception("Product not found.");
			}

			return coffee;
		}

		// Delete a CoffeeAddIn by its ID
		public static List<CoffeeAddIns> Delete(Guid id)
		{
			var addIns = GetAll(appProductsFilePath);
			var addInToDelete = addIns.FirstOrDefault(x => x.Id == id);

			if (addInToDelete == null)
			{
				throw new Exception("Coffee add-in not found.");
			}

			addIns.Remove(addInToDelete);

			SaveAll(addIns, appDataDirectoryPath, appProductsFilePath);

			return addIns;
		}

		// Create a new CoffeeAddIn
		public static List<CoffeeAddIns> Create(Guid userId, string name, double price)
		{
			if (name == "")
			{
				throw new Exception("Please insert correct and valid input for each of the fields.");
			}

			if (price <= 0)
			{
				throw new Exception("Add a positive integer value to add a product item.");
			}

			name = name.Trim().ToLower();

			var addInList = GetAll(appProductsFilePath);

			// Check for existing product with the same name
			var productExists = addInList.Any(x => x.Name.ToLower() == name);

			if (productExists)
			{
				throw new Exception("Product with the same already exists, Please add any other title.");
			}

			var addIn = new CoffeeAddIns()
			{
				Id = Guid.NewGuid(), // Assign a unique ID
				Name = name,
				Price = price,
				CreatedBy = userId
			};

			addInList.Add(addIn);

			SaveAll(addInList, appDataDirectoryPath, appProductsFilePath);

			return addInList;
		}

		// Edit an existing CoffeeAddIn
		public static List<CoffeeAddIns> Edit(Guid id, string name, double price)
		{
			List<CoffeeAddIns> addInList = GetAll(appProductsFilePath);
			 var addInToUpdate = addInList.FirstOrDefault(x => x.Id == id);

			if (addInToUpdate == null)
			{
				throw new Exception("Add in not found.");
			}

			if (name == "")
			{
				throw new Exception("Please insert correct and valid input for each of the fields.");
			}

			if (price <= 0)
			{
				throw new Exception("Add a positive integer value to add a product item.");
			}
			addInToUpdate.Name = name.Trim();
			addInToUpdate.Price = price;

			SaveAll(addInList, appDataDirectoryPath, appProductsFilePath);

			return addInList;
		}
	}
}

