using Microsoft.AspNetCore.Mvc;
using Product_api.Model;
using Product_api.Repository;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: api/products
    [HttpGet("/getProducts")]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
        }
    }

    // GET: api/products/1
    [HttpGet("/getProduct/{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the product.");
        }
    }

    // POST: api/products
    [HttpPost("/addProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            if (product == null)
                return BadRequest();

            var productId = await _productRepository.InsertAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = productId }, product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
        }
    }

    // PUT: api/products/1
    [HttpPut("/updateProduct/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            if (product == null || product.Id != id)
                return BadRequest();

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return NotFound();

            var success = await _productRepository.UpdateAsync(product);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
        }
    }

    // DELETE: api/products/1
    [HttpDelete("/deleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var success = await _productRepository.DeleteAsync(id);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
        }
    }
}
