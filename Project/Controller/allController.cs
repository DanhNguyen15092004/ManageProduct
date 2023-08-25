using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project.Models;

namespace Project.Controller
{
    [Route("api/[controller]")]
    [ApiController] 

    public class allController : ControllerBase
    {
        private readonly ManageProductsContext _context;
        public allController(ManageProductsContext context)
        {
            _context = context;
        }
        //api/home 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            var allProduct = await _context.Products.ToListAsync();
            return Ok(allProduct);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = _context.Products.FindAsync(id);
            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewProduct([FromBody] Products prod)
        {
            try
            {
                var newProduct = new Products
                {
                    ProductsName = prod.ProductsName,
                    CurrentPrice = prod.CurrentPrice,
                    ProdType = prod.ProdType
                };

               await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();
                return Ok(newProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleteProduct = await _context.Products.FindAsync(id);
                _context.Products.Remove(deleteProduct);
                await _context.SaveChangesAsync();
                return Ok("DeleteSuccess");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);


            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditItem (Products product , int id)
        {
            var item = await _context.Products.FindAsync(id);
            try
            {
                item.ProductsName = product.ProductsName;
                item.CurrentPrice = product.CurrentPrice;
                item.ProdType = product.ProdType;
             await  _context.SaveChangesAsync();
                return Ok(item);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }




    }


}

