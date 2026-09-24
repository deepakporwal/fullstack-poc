using FullStackPoc.Api.Data;
using FullStackPoc.Api.DTOs;
using FullStackPoc.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FullStackPoc.Api.Controllers;
[ApiController, Authorize, Route("api/products")]
public sealed class ProductsController(MongoDbContext db, ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await db.Products.Find(_ => true).SortByDescending(x => x.CreatedAt).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return BadRequest(new { message = "Invalid product id." });
        var item = await db.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        return item is null ? NotFound(new { message = "Product not found." }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Price < 0) return BadRequest(new { message = "Valid name and non-negative price are required." });
        var item = new Product { Name = request.Name, Price = request.Price };
        await db.Products.InsertOneAsync(item);
        logger.LogInformation("Product created {ProductId} by {User}", item.Id, User.Identity?.Name);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await db.Products.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0 ? NotFound(new { message = "Product not found." }) : NoContent();
    }
}
