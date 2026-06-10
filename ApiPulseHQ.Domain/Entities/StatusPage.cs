namespace ApiPulseHQ.Domain.Entities
{
    public class StatusPage
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        // Your service layer expects "Title"
        public string Title { get; set; } = default!;

        // Your service layer expects "Slug"
        public string Slug { get; set; } = default!;

        // Optional description
        public string? Description { get; set; }

        // Your service layer expects "CreatedAt"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublic { get; set; } = true;

        // Your EF config expects "StatusPageServices"
        public ICollection<StatusPageService> StatusPageServices { get; set; }
            = new List<StatusPageService>();

        // Your EF config expects "User"
        public User User { get; set; } = default!;
    }
}
