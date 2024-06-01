using BisleriumCafeCourseWork.Data;
using BisleriumCafeCourseWork.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Services
{
    public class UserService : GenericService<User>
    {
		// Seed data constants for initial user creation
		public const string SeedUsername = "admin";
        public const string SeedEmail = "admin@admin.com";
        public const string SeedPassword = "admin";
		public const string SeedSpecialPassword = "Pass@123";

		// Static paths for data storage
		public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
        public static string appUsersFilePath = UtilityService.GetAppUsersFilePath();

		// Seeds an initial admin user if none exists
		public static void SeedUser()
        {

            var users = GetAll(appUsersFilePath).FirstOrDefault(x => x.Role == Role.Admin);

            if (users == null)
            {
                Create(Guid.Empty, SeedUsername, SeedEmail, SeedPassword, Role.Admin, SeedSpecialPassword);
            }
        }

		// Retrieves all users with the Member role
		public static List<User> GetMembers()
		{
			var users = GetAll(appUsersFilePath);
			var members = users.Where(x => x.Role == Role.Member).ToList();
			return members;
		}

		// Changes the password of a user, ensuring it's different from the current one
		public static User ChangePassword(Guid id, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword)
            {
                throw new Exception("New password must be different from current password.");
            }

            List<User> users = GetAll(appUsersFilePath);
            User user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool passwordIsValid = UtilityService.VerifyHash(currentPassword, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception("Incorrect current password.");
            }

            user.PasswordHash = UtilityService.HashSecret(newPassword);
            user.HasInitialPassword = false;
            SaveAll(users, appDataDirectoryPath, appUsersFilePath);


            return user;
        }

		// Retrieves a user by their ID
		public static User GetById(Guid Id)
		{
			var user = GetAll(appUsersFilePath).FirstOrDefault(x => x.Id == Id);

			return user;
		}

		// Authenticates a user for login
		public static User Login(string username, string password)
        {
            var loginErrorMessage = "Invalid username or password.";

            var users = GetAll(appUsersFilePath);

            var user = users.FirstOrDefault(x => x.Username.Trim().ToLower() == username.Trim().ToLower());

            if (user == null)
            {
                throw new Exception(loginErrorMessage);
            }

            bool passwordIsValid = UtilityService.VerifyHash(password, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception(loginErrorMessage);
            }

            return user;
        }

		// Checks the special password for admin users
		public static bool CheckAdminPassword(string password)
		{
			var loginErrorMessage = "Invalid username or password.";

			var users = GetAll(appUsersFilePath);

			var user = users.FirstOrDefault(x => x.Role == Role.Admin);

			if (user == null)
			{
				throw new Exception(loginErrorMessage);
			}

			bool passwordIsValid = UtilityService.VerifyHash(password, user.SpecialPassword);

			if (!passwordIsValid)
			{
				throw new Exception(loginErrorMessage);
			}

			return passwordIsValid;
		}

		// Creates a new user, handling various validations
		public static List<User> Create(Guid userId, string username, string email, string password, Role role, string specialPassword = null)
        {
            username = username.Trim();

            if (username == "" || email == "" || password == "")
            {
                throw new Exception("Please insert correct and valid input for each of the fields.");
            }

            var users = GetAll(appUsersFilePath);

            var usernameExists = users.Any(x => x.Username.ToLower() == username.ToLower());
			var emailExists = users.Any(x => x.Email == email);


			if (usernameExists)
            {
                throw new Exception("Username already exists. Please choose any other username.");
            }
			if (emailExists)
			{
				throw new Exception("Email already exists. Please choose any other email.");
			}
			var numberOfAdmins = users.Where(x => x.Role == Role.Admin).Count();

            if (numberOfAdmins ==1 && role == Role.Admin)
            {
                throw new Exception("System has already one admin");
            }

            var user = new User()
            {
                Username = username,
                Email = email,
                PasswordHash = UtilityService.HashSecret(password),
                Role = role,
                CreatedBy = userId,
            };

			if (!string.IsNullOrEmpty(specialPassword))
			{
				user.SpecialPassword = UtilityService.HashSecret(specialPassword);
			}


			users.Add(user);

            SaveAll(users, appDataDirectoryPath, appUsersFilePath);

            return users;
        }


		// Creates a new member user
		public static List<User> CreateMember(Guid userId, string username, string phone)
		{
			username = username.Trim();

			if (username == "" )
			{
				throw new Exception("Please insert coorect and valid input for each of the fields.");
			}

			var users = GetAll(appUsersFilePath);


			var usernameExists = users.Any(x => x.Username == username && x.Role == Role.Member);
			var phoneExists = users.Any(x => x.PhoneNumber == phone && x.Role == Role.Member);


			if (usernameExists)
			{
				throw new Exception("Username already exists. Please choose any other username.");
			}

			if (phoneExists)
			{
				throw new Exception("Phone number already exists. Please enter another number.");
			}


			var user = new User()
			{
				Username = username,
				Role = Role.Member,
                PhoneNumber = phone,
				CreatedBy = userId,
			};

			users.Add(user);

			SaveAll(users, appDataDirectoryPath, appUsersFilePath);

            var members = GetMembers();

			return members;
		}

		// Deletes a user by their ID
		public static List<User> Delete(Guid id)
        {
            var users = GetAll(appUsersFilePath);

            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            users.Remove(user);

            SaveAll(users, appDataDirectoryPath, appUsersFilePath);

            return users;
        }
    }
}
