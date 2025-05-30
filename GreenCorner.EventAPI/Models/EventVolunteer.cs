using System;
using System.Collections.Generic;

namespace GreenCorner.EventAPI.Models;

public partial class EventVolunteer
{
    public int EventVolunteerId { get; set; }

    public int CleanEventId { get; set; }

    public string UserId { get; set; } = null!;

    public bool? IsTeamLeader { get; set; }

    public string? AttendanceStatus { get; set; }

    public int? PointsAwarded { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Note { get; set; }

    public virtual CleanupEvent CleanEvent { get; set; } = null!;
}
