using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class ClubRegistrationRequest
{
    public int RequestId { get; set; }

    public string ClubName { get; set; } = null!;

    public string PresidentStudentId { get; set; } = null!;

    public string PresidentFullName { get; set; } = null!;

    public string PresidentUsername { get; set; } = null!;

    public DateTime RequestedAt { get; set; }

    public int Status { get; set; }
}
