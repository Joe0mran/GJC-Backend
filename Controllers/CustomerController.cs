using GJC.Data;
using GJC.DTOs.Customers;
using GJC.Models;
using GJC.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace GJC.Controllers;

[Route("api/customer")]
[ApiController]

public class CustomerController : ControllerBase
{
    private readonly GJDbContext _GJDB;
    public CustomerController(GJDbContext GJDB)
    {
        _GJDB = GJDB;
    }

    // Gets

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
    {
        var customers = await _GJDB.Customers
            .AsNoTracking()
            .Include(c => c.Address)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync();

        return Ok(customers.Select(c => c.ToCustomerDTO()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromRoute] int id)
    {
        var Customer = await _GJDB.Customers.AsNoTracking().Include(c => c.Address).FirstOrDefaultAsync(c => c.CustomerId == id);
        if(Customer == null) return NotFound();
        return Ok(Customer.ToCustomerDTO());
    }


    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomer([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Ok(Array.Empty<CustomerDTO>());
        }

        q = q.Trim();
        if (q.Length < 2)
        {
            return Ok(Array.Empty<CustomerDTO>());
        }

        var Customers = await _GJDB.Customers.AsNoTracking().Include(c => c.Address)
        .Where(c =>
            EF.Functions.Like(c.PhoneNumber, $"%{q}%") ||
            (c.IgAccount != null && EF.Functions.Like(c.IgAccount, $"%{q}%")) ||
            EF.Functions.Like(c.FirstName, $"%{q}%") ||
            EF.Functions.Like(c.LastName, $"%{q}%")
        )
        .OrderBy(c => c.FirstName)
        .ThenBy(c => c.LastName)
        .Take(20)
        .ToListAsync();
        return Ok(Customers.Select(c => c.ToCustomerDTO()));
    }

    // Posts

    [HttpPost]
    public async Task<ActionResult<CustomerDTO>> CheckAndAdd([FromBody] NewCustomerDTO newCustomer)
    {
        if (string.IsNullOrWhiteSpace(newCustomer.PhoneNumber))
        {
            return BadRequest();
        }
        var found = await _GJDB.Customers.AnyAsync( c => 
            (c.PhoneNumber == newCustomer.PhoneNumber)
        );
        if (found)
        {
            return Conflict("Customer Already Exists");
        }
        // 1) create customer
        var customer = newCustomer.ToNewCustomer();
        await _GJDB.Customers.AddAsync(customer);
        await _GJDB.SaveChangesAsync(); // gets CustomerId

        // 2) create address (1-to-1, PK=CustomerId)
        var address = newCustomer.ToNewAddress(customer.CustomerId);
        await _GJDB.Addresses.AddAsync(address);
        await _GJDB.SaveChangesAsync();

        // return created dto
        customer.Address = address;
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer.ToCustomerDTO());
    }

    // Puts

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<CustomerDTO>> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerDTO updatedCustomer)
    {
        var customerModel = await _GJDB.Customers.Include(c => c.Address).FirstOrDefaultAsync(c => c.CustomerId == id);
        if(customerModel == null) return NotFound();
        updatedCustomer.ApplyUpdate(customerModel);

        // update/create address fields
        if (customerModel.Address == null)
        {
            customerModel.Address = new Address
            {
                CustomerId = customerModel.CustomerId,
                Country = updatedCustomer.Country,
                City = updatedCustomer.City,
                AddressLine = updatedCustomer.AddressLine,
                AddressNotes = updatedCustomer.AddressNotes
            };
            _GJDB.Addresses.Add(customerModel.Address);
        }
        else
        {
            updatedCustomer.ApplyAddressUpdate(customerModel.Address);
        }

        await _GJDB.SaveChangesAsync();
        return Ok(customerModel.ToCustomerDTO());
    }

    // Delete
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<CustomerDTO>> DeleteCustomer([FromRoute] int id)
    {
        var customer = await _GJDB.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        if(customer == null) return NotFound();
        _GJDB.Customers.Remove(customer);
        await _GJDB.SaveChangesAsync();
        return NoContent();
    }
}