using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Location { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public int ClubId { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    public bool IsUserRegistered(User user)
    {
        return EventRegistrations
            .Any(er => er.StudentId == user.StudentId);
    }

    public int RegisteredUsersCount => EventRegistrations.Count;
}
