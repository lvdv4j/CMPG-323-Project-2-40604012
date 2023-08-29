using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cmpg323_project.Models;

namespace cmpg323_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly cmpg323sqldbserverContext _context;

        public OrdersController(cmpg323sqldbserverContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(short orderId)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // GET: api/Orders/5
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<Order>> GetOrdersForCustomer(short customerId)
        {
            var orders = await _context.Orders
        .Where(o => o.CustomerId == customerId)
        .ToListAsync();

            if (orders.Count == 0)
            {
                return NotFound(); // No orders found for the specified customer
            }

            return Ok(orders);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{orderId}, {orderDate}, {customerId}, {deliveryAddress}")]
        public async Task<IActionResult> PutOrder(short orderId, DateTime orderDate, short customerId, string deliveryAddress)
        {
            Order order = new Order();

            if (orderId != order.OrderId)
            {
                return BadRequest();
            }

            order.OrderDate = orderDate;
            order.CustomerId = customerId;
            order.DeliveryAddress = deliveryAddress;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderId))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost ("{orderId}, {orderDate}, {customerId}, {deliveryAddress}")]
        public async Task<ActionResult<Order>> PostOrder(short orderId, DateTime orderDate, short customerId, string deliveryAddress)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'cmpg323sqldbserverContext.Orders'  is null.");
          }

            Order order = new Order();

            order.OrderId = orderId;
            order.OrderDate = orderDate;
            order.CustomerId = customerId;
            order.DeliveryAddress = deliveryAddress;

            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(short orderId)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            if (!OrderExists(orderId))
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(short id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
