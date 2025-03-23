using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class ChatChannel
{
    public int ChannelId { get; set; }

    public string ChannelName { get; set; } = null!;

    public int ClubId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public virtual Club Club { get; set; } = null!;
}
