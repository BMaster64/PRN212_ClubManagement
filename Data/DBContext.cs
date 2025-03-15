using Microsoft.EntityFrameworkCore;

public class DBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Club> Clubs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=DESKTOP-7VSQ595\\SQLEXPRESS;uid=lann;password=982Lan;Database=PRNProject;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map User entity to [User] table (with square brackets because User is a SQL keyword)
        modelBuilder.Entity<User>().ToTable("User");

        // Map Club entity to Club table
        modelBuilder.Entity<Club>().ToTable("Club");

        base.OnModelCreating(modelBuilder);

        // Configure User-Club relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Club)
            .WithMany()
            .HasForeignKey(u => u.ClubId)
            .OnDelete(DeleteBehavior.Restrict);
    }


    public int GetNextClubId()
    {
        // If there are no clubs, start with club ID 1
        if (!Clubs.Any())
        {
            return 1;
        }

        // Get the highest ClubId currently in use
        int maxClubId = Clubs.Max(c => c.ClubId);
        return maxClubId + 1;
    }

    // Method to create new club and return its ID
    public async Task<int> CreateClubAsync(Club club)
    {
        Clubs.Add(club);
        await SaveChangesAsync();
        return club.ClubId;
    }
}
