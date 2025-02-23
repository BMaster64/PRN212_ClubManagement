using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public class AuthService
{
    public bool Register(string name, string phone, string email, string password)
    {
        using var db = new DBContext();

        if (db.Users.Any(u => u.Email == email))
            return false; // Email already exists

        var hashedPassword = HashPassword(password);
        var user = new User
        {
            Name = name,
            Phone = phone,
            Email = email,
            Password = hashedPassword,
            UserType = 1
        };

        db.Users.Add(user);
        db.SaveChanges();
        return true;
    }

    public User? Login(string email, string password)
    {
        using var db = new DBContext();

        var user = db.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.Password))
            return null;

        string role = user.UserType switch
        {
            1 => "Chủ nhiệm",
            2 => "Phó chủ nhiệm",
            3 => "Trưởng ban",
            4 => "Thành viên",
            _ => "Unknown"
        };

        MessageBox.Show($"Login successful!\nRole: {role}");
        return user;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return HashPassword(enteredPassword) == storedHash;
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        using var db = new DBContext();
        return await db.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> PhoneExistsAsync(string phone)
    {
        using var db = new DBContext();
        return await db.Users.AnyAsync(u => u.Phone == phone);
    }

    public async Task<bool> RegisterAsync(User user)
    {
        using var db = new DBContext();
        try
        {
            user.Password = HashPassword(user.Password); // Hash the password before saving
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

