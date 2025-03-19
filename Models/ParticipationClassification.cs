using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class ParticipationClassification
{
    public int ParticipationClassificationId { get; set; }

    public string StudentId { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public int Year { get; set; }

    public int TotalEventsParticipated { get; set; }

    public int TotalEvents { get; set; }

    public virtual User Student { get; set; } = null!;
}
