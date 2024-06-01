using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Services
{
	// UtilityService class providing various utility methods
	public class UtilityService
    {
		// Delimiter used to separate segments in the hashed secret
		private const char _segmentDelimiter = ':';

        public static string HashSecret(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            var algorithm = HashAlgorithmName.SHA256;
            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            var result = string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );

            return result;
        }

		// Verifies if the input matches the hashed secret
		public static bool VerifyHash(string input, string hashString)
        {
            var segments = hashString.Split(_segmentDelimiter);
            var hash = Convert.FromHexString(segments[0]);
            var salt = Convert.FromHexString(segments[1]);
            var iterations = int.Parse(segments[2]);
            var algorithm = new HashAlgorithmName(segments[3]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

		// Gets the path of the application directory
		public static string GetAppDirectoryPath()
        {
            return @"D:\cw  bislerium\BisleriumCafeCourseWork\BisleriumCafeCourseWork\wwwroot\data";
        }

		// Gets the file path for storing user data
		public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

		// Gets the file path for storing coffee data
		public static string GetAppCoffeeFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "coffee.json");
        }

		// Gets the file path for storing coffee add-ins data
		public static string GetAppCoffeeAddInsFilePath()
		{
			return Path.Combine(GetAppDirectoryPath(), "coffeeaddin.json");
		}

		// Gets the file path for storing coffee orders data
		public static string GetAppCoffeeOrdersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "coffeeorders.json");
        }
    }
}
