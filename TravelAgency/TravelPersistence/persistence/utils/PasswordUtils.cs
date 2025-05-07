namespace TravelPersistence.persistence.utils;

public static class PasswordUtils
{
    public static string EncryptPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string inputPassword, string storedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
    }
}