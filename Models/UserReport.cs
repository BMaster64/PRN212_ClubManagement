using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class UserReport
{
    public int UserReportId { get; set; }

    public int ReportId { get; set; }

    public string StudentId { get; set; } = null!;

    public bool? IsRead { get; set; }

    public int ClubId { get; set; }

    public virtual Report Report { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
