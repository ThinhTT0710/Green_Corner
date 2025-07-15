using System;
using System.Collections.Generic;

namespace GreenCorner.EventAPI.Models;

public partial class CleanupEvent
{
    public int CleanEventId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? MaxParticipants { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? PointsAward { get; set; }

    public string? Address { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<EventReview> EventReviews { get; set; } = new List<EventReview>();

    public virtual ICollection<EventVolunteer> EventVolunteers { get; set; } = new List<EventVolunteer>();

    public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
}
