using BlazorApp1.Data;
using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MyApp.Data.Models;



namespace BlazorApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            builder.Services.AddHttpClient();
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddHttpClient();
            // dùng DI ?? ??ng ký Th?ng WeatherForescastServices
            builder.Services.AddSingleton<WeatherForecastService>();
            // truy c?p DB

            builder.Services.AddDbContextFactory<ManageProductsContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
            );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}