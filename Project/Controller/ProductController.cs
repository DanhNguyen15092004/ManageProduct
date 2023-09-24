using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project.Models;
using System.Net.WebSockets;
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
        public record ProductList(int Id,string ProductsName, string CurrentPrice, string ProdType,List<HistoryPriceItem> history);
        public record HistoryPriceItem (string OldPrices, DateTime DateChange);

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
                                         select new HistoryPriceItem(
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

   
        
        public record ProductItemWithoutHistory(string ProductsName,string CurrentPrice, string ProdType,DateTime DateCreate);
        [HttpPost("newItem")]
        public async Task<ActionResult<Result>> CreateProduct([FromBody] Products prod)
        {
            try
            {
                var item =  new Products{
                     ProductsName=  prod.ProductsName,
                     CurrentPrice =  prod.CurrentPrice,
                     ProdType = prod.ProdType,
                     DateCreate =  DateTime.Now
                };
                await _context.Products.AddAsync(item);
                await _context.SaveChangesAsync();
                return Ok(new Result { Success = " Tạo thành công " });
            }
            catch (Exception e)
            {
                _log.LogInformation($"Có lỗi nên tạo thất bại {e.Message}");
                return Ok(new Result { Error = new (message :$"{e.Message}")});
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteProduct(int id)
        {
            try
            {
                var deleteItem =await (from a in _context.Products
                                  where a.StatusProduct == 1 && a.Id == id
                                  select a).FirstOrDefaultAsync();


                if (deleteItem == null) return Ok(new Result { Error = new(message : "Không tìm thấy vật phẩm ")});


                deleteItem.StatusProduct = 0;
              

                var deleteHistoryItem =await (from b in _context.HistoryPrice
                                         where b.StatusHistory == 1 && b.IdProduct == id
                                         select b).FirstOrDefaultAsync();

                if (deleteHistoryItem != null)
                {
                    deleteHistoryItem.StatusHistory = 0;   
                }



                await _context.SaveChangesAsync();
                return Ok(new Result { Success = "Đã xóa thành công" });
            }
            catch (Exception e)
            {
                _log.LogError(e, $"Không thể xóa sản phẩm {e.Message}");
                return Ok(new Result { Error = new(message: "Đã có lỗi xảy ra") });
            }
        }


       
        [HttpPost("editName")]
        public async Task<ActionResult<Result>> EditName([FromBody] Products prod)
        {
            try
            {
                var changeName = await (from a in _context.Products
                                        where a.Id == prod.Id && a.StatusProduct == 1
                                        select a).FirstOrDefaultAsync();


                if (changeName == null) return Ok(new Result { Error = new(message: " Không tìm thấy sản phẩm này ") });

                changeName.ProductsName = prod.ProductsName;
                _context.SaveChanges();
                return Ok(new Result { Success = "Thành công thay đổi tên " });
            }catch(Exception e)
            {
                _log.LogError(e, "Không thể tìm thấy sản phẩm này ");
                return Ok(new Result { Error = new(message: "Đã có lỗi k thể tìm kiếm sản phẩm") });
            }
          

            

        }

    
        [HttpPost("editType")]
        public async Task<ActionResult<Result>> EditType([FromBody] Products prod)
        {
            try
            {
                var changeType = await (from a in _context.Products
                                        where a.Id == prod.Id && a.StatusProduct == 1
                                        select a).FirstOrDefaultAsync();


                if (changeType == null) return Ok(new Result { Error = new(message: " Không tìm thấy sản phẩm này ") });

                changeType.ProdType = prod.ProdType;
                _context.SaveChanges();
                return Ok(new Result { Success = "Thành công thay đổi tên " });
            }
            catch (Exception e)
            {
                _log.LogError(e, $"Không thể tìm thấy sản phẩm này {e.Message} ");
                return Ok(new Result { Error = new(message: "Đã có lỗi k thể tìm kiếm sản phẩm") });
            }

        }
    

        [HttpPost("editPrice")]

        public async Task<ActionResult<Result>> EditPrice(Products prod) 
        {
            try
            {
            var item =await (from a in _context.Products
                        where a.Id == prod.Id && a.StatusProduct == 1
                        select a).FirstOrDefaultAsync();


            if (item == null) return Ok(new Result { Error = new(message: "KHông thể tìm thấy vật phẩm này ") });
                var newHistoryProduct = new HistoryPrice
                {
                    OldPrice = item.CurrentPrice,
                    DateChange = DateTime.Now,
                    StatusHistory = 1 , 
                    IdProduct = prod.Id,
                    CurrentPrice = prod.CurrentPrice
                };
                item.CurrentPrice = prod.CurrentPrice;
                 await  _context.HistoryPrice.AddAsync(newHistoryProduct);
                await  _context.SaveChangesAsync();
                return Ok(new Result { Success = " Đã thay đổi thành công " });
            }

            catch (Exception e)
            {
                _log.LogError(e, $"Không thể thay đổi giá tiền của sản phẩm {e.Message}");
                return Ok(new Result { Error = new(message: "Có sự cố về mạng hãy thử lại ") });
            }
        }   

    }



}
