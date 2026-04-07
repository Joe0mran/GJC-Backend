namespace GJC.DTOs.Customers;

public class CustomerDTO
{
    // Customer Info
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; }
    public string? IgAccount { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    // Address Info
    public int? AddressId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? AddressLine { get; set; }
    public string? AddressNotes { get; set; }
    public bool? IsDefault { get; set; }
}
