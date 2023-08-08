using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Repositories;

namespace ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProductDb>(opt => opt.UseInMemoryDatabase("tbProducts"));
            builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
            var app = builder.Build();

            var productApiGroup = app.MapGroup("products");

            productApiGroup.MapGet("/", GetAllProducts);
            productApiGroup.MapGet("/{id}", GetProduct);
            productApiGroup.MapPost("/", CreateProduct);
            productApiGroup.MapPut("/{id}", UpdateProduct);
            productApiGroup.MapDelete("/{id}", DeleteProduct);

            app.Run();
        }

        private static async Task<IResult> DeleteProduct(int id, IRepository<Product> repository)
        {
            if (await repository.GetByIdAsync(id) is { } product)
            {
                await repository.DeleteAsync(id);
                return TypedResults.Ok(product);
            }

            return TypedResults.NotFound();
        }

        private static async Task<IResult> UpdateProduct(int id, Product detailsToUpdate,
            IRepository<Product> repository)
        {
            if (await repository.GetByIdAsync(id) != null)
            {
                await repository.UpdateAsync(id, detailsToUpdate);
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        private static async Task<IResult> CreateProduct(Product product, IRepository<Product> repository)
        {
            try
            {
                await repository.CreateAsync(product);
                return TypedResults.Created($"/products/{product.Id}", product);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> GetProduct(int id, IRepository<Product> repository)
        {
            return await repository.GetByIdAsync(id)
                is { } product
                ? TypedResults.Ok(product)
                : TypedResults.NotFound();
        }

        private static async Task<IResult> GetAllProducts(IRepository<Product> repository)
        {
            return TypedResults.Ok(await repository.GetAllAsync());
        }
    }
}