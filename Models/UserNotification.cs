using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class UserNotification
{
    public int UserNotificationId { get; set; }

    public int NotificationId { get; set; }

    public string StudentId { get; set; } = null!;

    public bool? IsRead { get; set; }

    public int ClubId { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
