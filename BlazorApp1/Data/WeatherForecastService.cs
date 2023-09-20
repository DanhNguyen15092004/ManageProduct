    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyApp.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;
    using static System.Net.WebRequestMethods;
   


namespace BlazorApp1.Data
{
    public record ProductList(List<ProductItem> Products);
    public record ProductItem(string ProductsName, string CurrentPrice, string ProdType, int StatusProduct, List<ProductItemHistory> HistoryList);
    public record ProductItemHistory(DateTime TimeCreated, string OldPrice);
    public record CommonErrorModel(string Code = default, string Message = default);

    public class Result
    {
        public bool Ok => Error == null;
        public object Data { get; set; }
        public CommonErrorModel Error { get; set; }
        public string Success { get; set; }
    }
 
    public class Result<T> : Result
    {
        public new T Data { get; set; }
    }
    public class WeatherForecastService
    {

        private readonly IDbContextFactory<ManageProductsContext> _dbContextFactory;

        public WeatherForecastService(IDbContextFactory<ManageProductsContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;

        }

        public async Task<Result<ProductList>> GetProducts()
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var products = await (from a in db.Products
                                  where a.StatusProduct == 1
                                  select new
                                  {
                                      a.Id,
                                      a.ProductsName,
                                      a.CurrentPrice,
                                      a.DateCreate,
                                      a.ProdType,
                                      a.StatusProduct
                                  }).ToListAsync();
            var productIds = products.Select(x => x.Id).ToList();

            if (productIds.Any() is false) return new Result<ProductList> { Error = new(Message: "No data") };

            var productHistoryList = await (from a in db.HistoryPrice
                                            where productIds.Contains(a.idProduct)
                                            select new
                                            {
                                                a.idProduct,
                                                a.OldPrice,
                                                a.DateChange
                                            }).ToListAsync();

            var result = products.Select(x =>
            {
                var history = productHistoryList.Where(y => y.idProduct == x.Id).Select(x => new ProductItemHistory(x.DateChange, x.OldPrice)).ToList();
                return new ProductItem(x.ProductsName, x.CurrentPrice, x.ProdType, x.StatusProduct, history);
            }).ToList();
            return new Result<ProductList> { Data = new ProductList(result) };
        }

    }
}