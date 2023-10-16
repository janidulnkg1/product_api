using Microsoft.AspNetCore.Mvc;
using Product_api.Model;
using Product_api.Repository;
using Product_api.Logging;
using System;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IfileLogger _logger;

    public ProductController(IProductRepository productRepository, IfileLogger statusLogging)
    {
        _productRepository = productRepository;
        _logger = statusLogging;
    }

    // GET: api/products
    [HttpGet("/getProducts")]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            _logger.Log("All Available Products are displayed.");
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.Log("Error: " + ex);
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
            {
                _logger.Log("Error: No Product found!");
                return NotFound();
            }
            _logger.Log($"Product with id:{id} found.");
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.Log("Error: " + ex);
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
            {
                _logger.Log("Error: Invalid product data.");
                return BadRequest();
            }

            var productId = await _productRepository.InsertAsync(product);
            _logger.Log($"Product created with ID: {productId}");
            return CreatedAtAction(nameof(GetProduct), new { id = productId }, product);
        }
        catch (Exception ex)
        {
            _logger.Log("Error: " + ex);
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
            {
                _logger.Log("Error: Invalid product data or ID mismatch.");
                return BadRequest();
            }

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                _logger.Log("Error: Product not found for update.");
                return NotFound();
            }

            var success = await _productRepository.UpdateAsync(product);
            if (success)
            {
                _logger.Log($"Product with ID {id} updated successfully.");
                return NoContent();
            }
            else
            {
                _logger.Log("Error: Failed to update the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }
        catch (Exception ex)
        {
            _logger.Log("Error: " + ex);
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
            {
                _logger.Log("Error: No Product found!");
                return NotFound();
            }

            var success = await _productRepository.DeleteAsync(id);
            if (success)
            {
                _logger.Log($"Product with ID {id} deleted successfully.");
                return NoContent();
            }
            else
            {
                _logger.Log("Error: Failed to delete the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }
        catch (Exception ex)
        {
            _logger.Log("Error: " + ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
        }
    }
}
