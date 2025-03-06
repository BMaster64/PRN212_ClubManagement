using Microsoft.EntityFrameworkCore;

public class DBContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=DESKTOP-7VSQ595\\SQLEXPRESS;uid=lann;password=982Lan;Database=PRNProject;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    public int GetNextClubId()
    {
        // If there are no users, start with group 1
        if (!Users.Any())
        {
            return 1;
        }

        // Get the highest GroupId currently in use
        int maxClubId = Users.Max(u => u.ClubId);
        return maxClubId + 1;
    }

}
