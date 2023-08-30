using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using cmpg323_project.Models;
using cmpg323_project.DTO;

namespace cmpg323_project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly cmpg323sqldbserverContext _context;

        public CustomersController(cmpg323sqldbserverContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersDTO>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();

            if (_context.Customers == null)
            {
                return NotFound();
            }

            var customersDTO = customers.Select(customer => new CustomersDTO
            {
                CustomerId = customer.CustomerId,
                CustomerTitle = customer.CustomerTitle,
                CustomerName = customer.CustomerName,
                CustomerSurname = customer.CustomerSurname,
                CellPhone = customer.CellPhone
            }).ToList();

            return customersDTO;
        }

        // GET: api/Customers/5
        [HttpGet("{customerID}")]
        public async Task<ActionResult<CustomersDTO>> GetCustomer(short customerID)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(customerID);

            if (customer == null)
            {
                return NotFound();
            }

            var customersDTO = new CustomersDTO
            {
                CustomerId = customer.CustomerId,
                CustomerTitle = customer.CustomerTitle,
                CustomerName = customer.CustomerName,
                CustomerSurname = customer.CustomerSurname,
                CellPhone = customer.CellPhone
            };

            return customersDTO;
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomersDTO customerDTO)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'project2sqldbContext.Customers' is null.");
            }

            var customer = new Customer
            {
                CustomerId = customerDTO.CustomerId,
                CustomerTitle = customerDTO.CustomerTitle,
                CustomerName = customerDTO.CustomerName,
                CustomerSurname = customerDTO.CustomerSurname,
                CellPhone = customerDTO.CellPhone
            };

            _context.Customers.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            // Create the corresponding DTO for the response
            var createdCustomerDTO = new CustomersDTO
            {
                CustomerId = customer.CustomerId,
                CustomerTitle = customer.CustomerTitle,
                CustomerName = customer.CustomerName,
                CustomerSurname = customer.CustomerSurname,
                CellPhone = customer.CellPhone
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, createdCustomerDTO);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(short id, CustomersDTO customerDTO)
        {
            if (id != customerDTO.CustomerId)
            {
                return BadRequest();
            }

            if(!CustomerExists(id))
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            customer.CustomerTitle = customerDTO.CustomerTitle;
            customer.CustomerName = customerDTO.CustomerName;
            customer.CustomerSurname = customerDTO.CustomerSurname;
            customer.CellPhone = customerDTO.CellPhone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{customerID}")]
        public async Task<IActionResult> DeleteCustomer(short customerID)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            if (!CustomerExists(customerID))
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(customerID);

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(short id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}