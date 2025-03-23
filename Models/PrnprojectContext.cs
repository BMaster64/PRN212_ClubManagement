using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PRN212_Project.Models;

public partial class PrnprojectContext : DbContext
{
    public PrnprojectContext()
    {
    }

    public PrnprojectContext(DbContextOptions<PrnprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatChannel> ChatChannels { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventRegistration> EventRegistrations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ParticipationClassification> ParticipationClassifications { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserNotification> UserNotifications { get; set; }

    public virtual DbSet<UserReport> UserReports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatChannel>(entity =>
        {
            entity.HasKey(e => e.ChannelId).HasName("PK__ChatChan__38C3E814A634A7CE");

            entity.ToTable("ChatChannel");

            entity.Property(e => e.ChannelName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Club).WithMany(p => p.ChatChannels)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatChannel_Club");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C0C9C76703D09");

            entity.ToTable("ChatMessage");

            entity.Property(e => e.SenderId).HasMaxLength(50);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Channel).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatMessage_Channel");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatMessage_User");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Club__D35058E748BEBC16");

            entity.ToTable("Club");

            entity.Property(e => e.ClubName).HasMaxLength(255);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__7944C8103D99A176");

            entity.ToTable("Event");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.EventName).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Club).WithMany(p => p.Events)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_Club");
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.HasKey(e => e.EventRegistrationId).HasName("PK__EventReg__83225A925E313E9B");

            entity.ToTable("EventRegistration");

            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasMaxLength(50);

            entity.HasOne(d => d.Event).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventRegistration_Event");

            entity.HasOne(d => d.Student).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventRegistration_User");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12776AF204");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Sender).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Sender");
        });

        modelBuilder.Entity<ParticipationClassification>(entity =>
        {
            entity.HasKey(e => e.ParticipationClassificationId).HasName("PK__Particip__6492A5779B197412");

            entity.ToTable("ParticipationClassification");

            entity.Property(e => e.Semester).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasMaxLength(50);

            entity.HasOne(d => d.Student).WithMany(p => p.ParticipationClassifications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParticipationClassification_User");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48059F17A336");

            entity.ToTable("Report");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Club).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK_Report_Club");

            entity.HasOne(d => d.Sender).WithMany(p => p.Reports)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Sender");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__User__32C52B99B2C6FE3B");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4F1DEB3E8").IsUnique();

            entity.Property(e => e.StudentId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Club).WithMany(p => p.Users)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Club");
        });

        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.HasKey(e => e.UserNotificationId).HasName("PK__UserNoti__EB29862938022D92");

            entity.ToTable("UserNotification");

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.StudentId).HasMaxLength(50);

            entity.HasOne(d => d.Notification).WithMany(p => p.UserNotifications)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserNotification_Notification");

            entity.HasOne(d => d.Student).WithMany(p => p.UserNotifications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserNotification_User");
        });

        modelBuilder.Entity<UserReport>(entity =>
        {
            entity.HasKey(e => e.UserReportId).HasName("PK__UserRepo__915FD7F5B90CE2EB");

            entity.ToTable("UserReport");

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.StudentId).HasMaxLength(50);

            entity.HasOne(d => d.Report).WithMany(p => p.UserReports)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReport_Report");

            entity.HasOne(d => d.Student).WithMany(p => p.UserReports)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReport_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
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
