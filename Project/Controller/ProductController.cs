using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project.Models;
using System.Security.AccessControl;
using static Project.Controller.ProductController;

namespace Project.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly ManageProductsContext _context;
        private readonly ILogger<ProductController> _log;
        public ProductController(ManageProductsContext context ,ILogger<ProductController> log)
        {
            _context = context;
            _log = log;
        }


        public record ProductItem(int Id, string ProductsName, string CurrentPrice, string ProdType);
        public record ProductList(int Id,string ProductsName, string CurrentPrice, string ProdType,List<HistoryPrice> history);
        public record HistoryPrice(string OldPrices, DateTime DateChange);

        public class Result
        {
            private bool OK => Error == null;
            public object? data { get; set; }

            public string Success { get; set; }

            public CommondError Error { get; set; }

        }
        public class Result<T> : Result
        {
            public T data {get ; set;}
            
          
     }
        public record CommondError (string message);
        [HttpGet]
        public async Task<ActionResult<Result<List<ProductList>>>> GetAllProductItem()
        {
            try
            {
                var productItems = await (from a in _context.Products
                                          where a.StatusProduct == 1
                                          select new ProductItem(
                                              a.Id,
                                              a.ProductsName,
                                              a.CurrentPrice,
                                              a.ProdType
                                          )).ToListAsync();

                if (productItems.Any() is false)
                {
                    _log.LogInformation("No data to query");
                    return Ok(new Result { Error = new(message: "No data!") });
                }
                var productLists = new List<ProductList>();
                foreach (var item in productItems)
                {
                    var history = await (from b in _context.HistoryPrice
                                         where b.IdProduct == item.Id
                                         select new HistoryPrice(
                                             b.OldPrice,
                                             b.DateChange
                                         )).ToListAsync();
                    productLists.Add(new ProductList(
                        item.Id,
                        item.ProductsName,
                        item.CurrentPrice,
                        item.ProdType,
                        history
                    ));
                }
                _log.LogInformation("Log thanh cong ");
                return Ok(new Result<List<ProductList>>
                {
                    data = productLists,
                    Success = "Data retrieved successfully."
                    
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{ex.Message} ");
                return Ok(new Result { Error = new(message : $"Error: {ex.Message}") });
            }
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

