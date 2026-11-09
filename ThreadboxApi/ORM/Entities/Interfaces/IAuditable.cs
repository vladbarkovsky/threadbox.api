namespace ThreadboxApi.ORM.Entities.Interfaces
{
    internal interface IAuditable
    {
        DateTimeOffset CreatedAt { get; set; }
        string CreatedById { get; set; }
        ApplicationUser CreatedBy { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        string UpdatedById { get; set; }
        ApplicationUser UpdatedBy { get; set; }
    }
}