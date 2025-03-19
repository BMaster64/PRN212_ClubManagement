using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public static Club CreateNewClub(string clubName, string description)
    {
        return new Club
        {
            ClubName = clubName,
            Description = description
        };
    }
}
