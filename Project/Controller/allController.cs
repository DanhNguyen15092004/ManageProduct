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
        //api/all 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            var allProd = _context.Products.Where(prod => prod.StatusProduct == 1).Select
                (prod => new {prod.ProductsName,prod.CurrentPrice,prod.ProdType}) ; 
            return Ok(allProd);
        }
        //get each id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = _context.Products.Where
                (prod => prod.Id == id && prod.StatusProduct == 1)
                .Select(prod => new {prod.ProductsName ,prod.CurrentPrice, prod.ProdType });

            if (item == null)
            {
                return NotFound("Khong tim thay");
            }
            else
            {
                return Ok(item);
            }
        }
        // create new item 
        [HttpPost]
        public async Task<IActionResult> NewProduct([FromBody] Products prod)
        {
            try
            {
                var newProduct = new Products
                {
                    ProductsName = prod.ProductsName,
                    CurrentPrice = prod.CurrentPrice,
                    ProdType = prod.ProdType,
                    StatusProduct = 1 
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

        //delete one item 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleteProduct = await _context.Products.FindAsync(id);
                deleteProduct.StatusProduct = 0;
                await _context.SaveChangesAsync();
                return Ok("DeleteSuccess");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);


            }
        }

        //edit 1 item 
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

