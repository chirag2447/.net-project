using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class FriendRequest
{
    public int Id { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public string? Status { get; set; }

    public DateTime? SentDate { get; set; }

    public virtual User? Receiver { get; set; }

    public virtual User? Sender { get; set; }
}
