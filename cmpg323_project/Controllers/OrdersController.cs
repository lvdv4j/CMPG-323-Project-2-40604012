using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cmpg323_project.Models;
using Microsoft.AspNetCore.Authorization;
using cmpg323_project.DTO;
using Microsoft.AspNetCore.JsonPatch;

namespace cmpg323_project.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<OrdersDTO>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            // Create a list of OrderDTO objects
            var orderDTOs = orders.Select(order => new OrdersDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                DeliveryAddress = order.DeliveryAddress,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailsDTO
                {
                    //Create Order Details DTO Object
                    OrderDetailsId = detail.OrderDetailsId,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                }).ToList()
            }).ToList();

            return orderDTOs;
        }

        // GET: api/Orders/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrdersDTO>> GetOrder(short orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Create an OrderDTO object
            var orderDTO = new OrdersDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                DeliveryAddress = order.DeliveryAddress,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailsDTO
                {
                    OrderDetailsId = detail.OrderDetailsId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                }).ToList()
            };

            return orderDTO;
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

        [HttpPatch("{customerId}")]
        public async Task<IActionResult> PatchOrder(short customerId, JsonPatchDocument<Order> patchDocument)
        {
            var order = await _context.Orders.FindAsync(customerId);
            if (order == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(order, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(customerId))
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

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{orderId}")]
        public async Task<IActionResult> PutOrder(short orderId, OrdersDTO orderDTO)
        {
            if (!OrderExists(orderId))
            {
                return BadRequest("Order ID mismatch.");
            }

            var existingOrder = await _context.Orders.FindAsync(orderId);

            if (_context.Customers == null || _context.Products == null)
            {
                return Problem("Entity sets are null.");
            }

            if (existingOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            var customer = await _context.Customers.FindAsync(orderDTO.CustomerId);

            if (customer == null)
            {
                return NotFound($"Customer with ID {orderDTO.CustomerId} not found.");
            }

            // Update properties for order object
            existingOrder.OrderDate = orderDTO.OrderDate;
            existingOrder.CustomerId = orderDTO.CustomerId;
            existingOrder.DeliveryAddress = orderDTO.DeliveryAddress;

            // Remove any order details that already exist
            var existingOrderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
            _context.OrderDetails.RemoveRange(existingOrderDetails);

            //Update the order details
            foreach (var orderDetailsDTO in orderDTO.OrderDetails)
            {
                var product = await _context.Products.FindAsync(orderDetailsDTO.ProductId);
                if (product == null)
                {
                    return NotFound($"Product with ID {orderDetailsDTO.ProductId} not found.");
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    OrderDetailsId = orderDetailsDTO.OrderId,
                    ProductId = orderDetailsDTO.ProductId,
                    Quantity = orderDetailsDTO.Quantity,
                    Discount = orderDetailsDTO.Discount
                };

                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrdersDTO ordersDTO)
        {
            if (_context.Customers == null || _context.Products == null)
            {
                return Problem("Customer or Products are null.");
            }

            var customer = await _context.Customers.FindAsync(ordersDTO.CustomerId);
            if (customer == null)
            {
                return NotFound($"Customer with ID {ordersDTO.CustomerId} could not be found.");
            }

            var order = new Order
            {
                OrderId = ordersDTO.OrderId,
                OrderDate = ordersDTO.OrderDate,
                CustomerId = ordersDTO.CustomerId,
                DeliveryAddress = ordersDTO.DeliveryAddress
            };

            foreach (var orderDetailsDTO in ordersDTO.OrderDetails)
            {
                var product = await _context.Products.FindAsync(orderDetailsDTO.ProductId);
                if (product == null)
                {
                    return NotFound($"Product with ID {orderDetailsDTO.ProductId} could not be found.");
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    OrderDetailsId = orderDetailsDTO.OrderDetailsId,
                    ProductId = orderDetailsDTO.ProductId,
                    Quantity = orderDetailsDTO.Quantity,
                    Discount = orderDetailsDTO.Discount
                };

                _context.OrderDetails.Add(orderDetail);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(short orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if ((!OrderExists(orderId)))
            {
                return NotFound($"Order with ID {orderId} could not be found.");
            }

            // Delete related OrderDetails first
            var relatedOrderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId);
            _context.OrderDetails.RemoveRange(relatedOrderDetails);

            // Remove the order itself
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
