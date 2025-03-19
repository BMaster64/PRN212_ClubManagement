using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public class AuthService
{
    public User? Login(string username, string password)
    {
        using var db = new PrnprojectContext();

        try
        {
            // First get the user without including Club to check credentials
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Status);

            if (user == null || !VerifyPassword(password, user.Password))
                return null;


            string role = user.RoleId switch
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
        catch (Exception ex)
        {
            MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
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

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var db = new PrnprojectContext();
        return await db.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> StudentIdExistsAsync(string studentId)
    {
        using var db = new PrnprojectContext();
        return await db.Users.AnyAsync(u => u.StudentId == studentId);
    }

    public async Task<bool> RegisterWithClubAsync(User user, Club club)
    {
        using var db = new PrnprojectContext();
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            // First, create the club
            db.Clubs.Add(club);
            await db.SaveChangesAsync();

            // Set the user's ClubId to the newly created club's ID
            user.ClubId = club.ClubId;
            user.Password = HashPassword(user.Password); // Hash the password before saving
            user.Status = true;

            // Now add the user
            db.Users.Add(user);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> RegisterAsync(User user)
    {
        using var db = new PrnprojectContext();
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