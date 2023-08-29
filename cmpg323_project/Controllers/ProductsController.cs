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
    public class ProductsController : ControllerBase
    {
        private readonly cmpg323sqldbserverContext _context;

        public ProductsController(cmpg323sqldbserverContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProduct(short productId)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<Product>> GetProductsForOrder(short orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound(); // Order not found for the specified order ID
            }

            var products = order.OrderDetails.Select(od => od.Product).ToList();

            return Ok(products);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productId}, {productName}, {productDescription}, {unitsInStock}")]
        public async Task<IActionResult> PutProduct(short productId, string productName, string productDescription, int unitsInStock)
        {
            Product product = new Product();

            product.ProductId = productId;

            if (productId != product.ProductId)
            {
                return BadRequest();
            }

            product.ProductId = productId;
            product.ProductName = productName;
            product.ProductDescription = productDescription;
            product.UnitsInStock = unitsInStock;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{productId}, {productName}, {productDescription}, {unitsInStock}")]
        public async Task<ActionResult<Product>> PostProduct(short productId, string productName, string productDescription, int unitsInStock)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'cmpg323sqldbserverContext.Products'  is null.");
          }

            Product product = new Product();

            product.ProductId = productId;
            product.ProductName = productName;
            product.ProductDescription = productDescription;
            product.UnitsInStock = unitsInStock;

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(short productId)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            if (!ProductExists(productId))
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }
            

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(short id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
