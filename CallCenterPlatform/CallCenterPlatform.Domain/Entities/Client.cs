using CallCenterPlatform.Domain.Enums;

namespace CallCenterPlatform.Domain.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public CompanyType CompanyType { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }  // Which Admin/Agent created
    
    // Navigation Property (if needed later)
    // public ICollection<Call> Calls { get; set; }
}