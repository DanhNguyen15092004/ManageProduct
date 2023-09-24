using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Project.Models;
using Serilog;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
           


                var builder = WebApplication.CreateBuilder(args);


                builder.Host.UseSerilog((context, configuration) => configuration
               .ReadFrom.Configuration(context.Configuration));
                



                builder.Services.AddDbContext<ManageProductsContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
                );

                builder.Services.AddControllers();



                builder.Services.AddCors(
                 option => option.AddDefaultPolicy(builder =>
                 {
                     builder.WithOrigins("http://127.0.0.1:5500", "https://a7686e0c.manageproduct.pages.dev").AllowAnyMethod()
                     .AllowAnyHeader().AllowCredentials(); ;
                 }
                 )
                 );


                builder.Services.AddRazorPages();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }
                app.UseCors();
                app.UseSerilogRequestLogging();
                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();
                app.UseAuthorization();

                // Configure routing and controllers here, outside of the environment check.
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });

                app.Run();
            }     
    }
}
