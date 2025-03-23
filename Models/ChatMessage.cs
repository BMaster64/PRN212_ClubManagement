using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class ChatMessage
{
    public int MessageId { get; set; }

    public string Content { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public int ChannelId { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual ChatChannel Channel { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
