using System.Text.Json;

namespace BikeSC.Data;

public static class UsersService
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    private static void SaveUser(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }
          
        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
      
    }

    public static List<User> GetAllUser()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    public static List<User> CreateUser(Guid userId, string username, string password, Role role)
    {
        List<User> users = GetAllUser();
        bool usernameExists = users.Any(x => x.Username == username);

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utils.HashedPassword(password),
                Role = role,
                CreatedBy = userId
            }
        );
        SaveUser(users);
        return users;
    }

    public static void DefaultAdminUser()
    {
        var users = GetAllUser().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            CreateUser(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }

    public static User GetUserByGuid(Guid id)
    {
        List<User> users = GetAllUser();
        return users.FirstOrDefault(x => x.Id == id);
    }

    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAllUser();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }
        users.Remove(user);
        SaveUser(users);

        return users;
    }

    public static User UserLogin(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAllUser();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHashedPassword(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }

    public static User ChangeUserPassword(Guid id, string currentPassword, string newPassword)
    {
        if (currentPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = GetAllUser();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        bool passwordIsValid = Utils.VerifyHashedPassword(currentPassword, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        user.PasswordHash = Utils.HashedPassword(newPassword);
        user.HasInitialPassword = false;
        SaveUser(users);

        return user;
    }
}
