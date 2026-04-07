using GJC.DTOs.Customers;
using GJC.Models;

namespace GJC.Mappers;

public static class CustomerMapper
{
    // CREATE: DTO -> Customer
    public static Customer ToNewCustomer(this NewCustomerDTO dto)
    {
        return new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            IgAccount = dto.IgAccount,
            Notes = dto.Notes,
            IsActive = true
        };
    }

    // UPDATE: DTO -> existing tracked Customer
    public static void ApplyUpdate(this UpdateCustomerDTO dto, Customer customer)
    {
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Email = dto.Email;
        customer.IgAccount = dto.IgAccount;
        customer.Notes = dto.Notes;
    }

    // CREATE: DTO -> Address (PK+FK = CustomerId)
    public static Address ToNewAddress(this NewCustomerDTO dto, int customerId)
    {
        return new Address
        {
            CustomerId = customerId,
            Country = dto.Country,
            City = dto.City,
            AddressLine = dto.AddressLine,
            AddressNotes = dto.AddressNotes
        };
    }

    // UPDATE: DTO -> existing tracked Address
    public static void ApplyAddressUpdate(this UpdateCustomerDTO dto, Address address)
    {
        address.Country = dto.Country;
        address.City = dto.City;
        address.AddressLine = dto.AddressLine;
        address.AddressNotes = dto.AddressNotes;
    }

    // READ: Entity -> DTO (single Address)
    public static CustomerDTO ToCustomerDTO(this Customer customer)
    {
        return new CustomerDTO
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            IgAccount = customer.IgAccount,
            Notes = customer.Notes,
            IsActive = customer.IsActive,

            Country = customer.Address?.Country,
            City = customer.Address?.City,
            AddressLine = customer.Address?.AddressLine,
            AddressNotes = customer.Address?.AddressNotes
        };
    }
}
