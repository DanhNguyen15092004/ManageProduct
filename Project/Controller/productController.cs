using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {
        private readonly ManageProductsContext _context;
        public productController(ManageProductsContext context)
        {
            _context = context;
        }
        //api/all 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            var allProd = await _context.Products.Where(prod => prod.StatusProduct == 1)
        .Select(prod => new { prod.ProductsName, prod.CurrentPrice, prod.DateCreate, prod.Id, prod.ProdType })
        .ToListAsync();
            return Ok(allProd);
        }
        //get each id
        [Route("api/product/{id}")]
        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            var item = _context.Products.Where
                (prod => prod.Id == id && prod.StatusProduct == 1)
                .Select(prod => new
                {
                    prod.ProductsName,
                    prod.CurrentPrice,
                    prod.ProdType,
                    prod.DateCreate
                });

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
        [Route("api/product/newItem")]
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
                    StatusProduct = 1,
                    DateCreate = DateTime.Now
                };
                //add History Prices 
                var newHistoryPrice = new HistoryPrice
                {
                    CurrentPrice = prod.CurrentPrice,
                    OldPrice = "0",
                    DateChange = DateTime.Now,
                    IdProduct = prod.Id,
                    StatusHistory = 1
                };

                await _context.Products.AddAsync(newProduct);
                await _context.HistoryPrice.AddAsync(newHistoryPrice);
                await _context.SaveChangesAsync();
                return Ok(newProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("api/product/deleteItem")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleteProduct = await _context.Products.FindAsync(id);
                var historyPrice = await _context.HistoryPrice.FirstOrDefaultAsync(prodId => prodId.IdProduct == id);

                deleteProduct.StatusProduct = 0;
                historyPrice.StatusHistory = 0;

                await _context.SaveChangesAsync();
                return Ok("DeleteSuccess");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //edit 1 item 
        [Route("api/product/edit")]
        [HttpPost()]
        public async Task<IActionResult> EditItem(Products product, [FromBody] int id)
        {

            var item = await _context.Products.FindAsync(id);

            //lấy những thằng có id bằng thằng product Id
            var HistoryPrices = _context.HistoryPrice.FirstOrDefault(history => history.IdProduct == id);

            try
            {
                //Làm thằng historyPrices trước để lấy giá cũ c
                ///còn tạo giá mới thì tính sau 
                HistoryPrices.CurrentPrice = product.CurrentPrice;
                HistoryPrices.OldPrice = item.CurrentPrice;
                HistoryPrices.DateChange = DateTime.Now;

                item.ProductsName = product.ProductsName;
                item.CurrentPrice = product.CurrentPrice;
                item.ProdType = product.ProdType;
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }


}

