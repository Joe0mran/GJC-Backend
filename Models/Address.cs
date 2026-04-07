using System.ComponentModel.DataAnnotations;

namespace GJC.Models;
public class Address
{
    [Key] public int CustomerId { get; set; }
    [Required] public string Country { get; set; } = default!;
    [Required] public string City { get; set; } = default!;
    [Required] public string AddressLine { get; set; } = default!;
    public string? AddressNotes { get; set; }
    public DateTime CreatedtAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Nav
    public Customer Customer { get; set; } = default!;
}