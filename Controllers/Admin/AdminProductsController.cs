using cl_be.Models;
using cl_be.Models.Dto.CustomerDto;
using cl_be.Models.Dto.ProductDto;
using cl_be.Models.Dto.ProductDto.Admin;
using cl_be.Models.Pagination;
using cl_be.Services;
using cl_be.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cl_be.Controllers.Admin
{
    [Route("api/admin/products")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IAdminProductService _adminProductService;

        public AdminProductsController(IAdminProductService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        [HttpGet]
        public async Task<ActionResult<Page<AdminProductListDto>>> GetAll(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 50, 
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? search = null
            )
        {
            var result = await _adminProductService.GetAllProductsAsync(page, pageSize, sortBy, sortDirection, search);
            return Ok(result);
        }

        [HttpGet("GetProductDetail/{productId}")]
        public async Task<ActionResult<AdminProductDetailDto>> GetProductDetail(int productId)
        {
            var dto = await _adminProductService.GetProductDetailsAsync(productId);
            return Ok(dto);
        }

        /// <summary>
        /// Admin: get product for edit/create form
        /// </summary>
        [HttpGet("GetProductToEdit/{productId}")]
        public async Task<ActionResult<AdminProductEditDto>> GetProductToEdit(int productId)
        {
            var product = await _adminProductService.GetProductToEditAsync(productId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<AdminProductCategoryDto>>> GetCategories()
        {
            var categories = await _adminProductService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<AdminProductModelDto>>> GetModels()
        {
            var models = await _adminProductService.GetModelsAsync();
            return Ok(models);
        }

        // PUT/ PATCH/ EDIT/ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] AdminProductUpdateDto dto)
        {
            /* 1. Route vs Body guard */
            if (id != dto.ProductId)
                return BadRequest("ProductId mismatch between route and body.");

            /* 2. Model validation (DTO rules) */
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                /* 3. Delegate to service layer */
                await _adminProductService.UpdateProductAsync(dto);
                return NoContent(); // 204
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // POST/ CREATE
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] AdminProductCreateDto dto)
        {
            var productId = await _adminProductService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(CreateProduct), new { id = productId }, productId);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                await _adminProductService.DeleteProductAsync(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error during the process of elimination: {ex.Message}"});
            }
        }

    }
}
