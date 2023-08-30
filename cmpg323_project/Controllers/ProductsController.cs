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
    public class ProductsController : ControllerBase
    {
        private readonly cmpg323sqldbserverContext _context;

        public ProductsController(cmpg323sqldbserverContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            //implement DTO list to create a product object
            var productDTOs = products.Select(product => new ProductsDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                UnitsInStock = product.UnitsInStock
            }).ToList();

            return productDTOs;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsDTO>> GetProduct(short productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            //create a productDTO object
            var productDTO = new ProductsDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                UnitsInStock = product.UnitsInStock
            };

            return productDTO;
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(short id, ProductsDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return BadRequest();
            }

            if (!ProductExists(id))
            {
                return NotFound();
            }

            // Get the product entity
            var product = await _context.Products.FindAsync(id);

            //Update the product using the DTO values
            product.ProductName = productDTO.ProductName;
            product.ProductDescription = productDTO.ProductDescription;
            product.UnitsInStock = productDTO.UnitsInStock;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        [HttpPost]
        public async Task<ActionResult<ProductsDTO>> PostProduct(ProductsDTO productDTO)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'project2sqldbContext.Products' is null.");
            }

            // Create a Product entity using the DTO values
            var product = new Product
            {
                ProductId = productDTO.ProductId,
                ProductName = productDTO.ProductName,
                ProductDescription = productDTO.ProductDescription,
                UnitsInStock = productDTO.UnitsInStock
            };

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

            // Create a ProductDTO object
            var createdProductDTO = new ProductsDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                UnitsInStock = product.UnitsInStock
            };

            return Ok(createdProductDTO);
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
