using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public string SenderId { get; set; } = null!;

    public int? ClubId { get; set; }

    public virtual Club? Club { get; set; }

    public virtual User Sender { get; set; } = null!;

    public virtual ICollection<UserReport> UserReports { get; set; } = new List<UserReport>();
}
