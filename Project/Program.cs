using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
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
