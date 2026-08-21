using CallCenterPlatform.Domain.Enums;

namespace CallCenterPlatform.Application.DTOs;

public class ClientDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateClientRequestDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyType { get; set; } = "SME";
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateClientRequestDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}