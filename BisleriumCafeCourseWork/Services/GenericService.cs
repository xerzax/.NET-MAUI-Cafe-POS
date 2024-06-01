using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Services
{
	// GenericService class that operates on a generic type T
	public class GenericService<T> where T : class
    {
		// Method to retrieve all entities from a file
		public static List<T> GetAll(string filePath)
        {
			// Check if the file exists; if not, return an empty list
			if (!File.Exists(filePath))
            {
                return new List<T>();
            }

			// Read the content of the file
			var json = File.ReadAllText(filePath);

			// Deserialize the JSON content to a list of type T and return it
			var result = JsonSerializer.Deserialize<List<T>>(json);

            return result;
        }

		// Method to save all entities to a file
		public static void SaveAll(List<T> entity, string directoryPath, string filePath)
        {
			// Check if the directory exists; if not, create it
			if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

			// Serialize the list of entities to a JSON string
			var json = JsonSerializer.Serialize(entity);

			File.WriteAllText(filePath, json); // Write the JSON string to the specified file path
		}

    }
}
