using System.ComponentModel.DataAnnotations;

namespace GJC.Models;

public class Customer{
    [Key]
    public int CustomerId { get; set; }
    [Required] public string FirstName { get; set; } = default!;
    [Required] public string LastName { get; set; } = default!;
    [Required] public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? IgAccount { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Nav
    public Address? Address { get; set; }
}
