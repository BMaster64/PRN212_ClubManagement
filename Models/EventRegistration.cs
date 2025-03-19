using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class EventRegistration
{
    public int EventRegistrationId { get; set; }

    public int EventId { get; set; }

    public string StudentId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
