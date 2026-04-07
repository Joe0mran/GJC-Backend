using System.ComponentModel.DataAnnotations;

namespace GJC.DTOs.Customers;

public class NewCustomerDTO{
    // Customer Info
    [Required] public string FirstName { get; set; } = default!;
    [Required] public string LastName { get; set; } = default!;
    [Required] public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? IgAccount { get; set; }
    public string? Notes { get; set; }
    // Address Info
    [Required] public string Country { get; set; } = default!;
    [Required] public string City { get; set; } = default!;
    [Required] public string AddressLine { get; set; } = default!;
    public string? AddressNotes { get; set; }
}