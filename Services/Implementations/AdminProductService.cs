using cl_be.Models;
using cl_be.Models.Dto.CustomerDto;
using cl_be.Models.Dto.ProductDto;
using cl_be.Models.Dto.ProductDto.Admin;
using cl_be.Models.Pagination;
using cl_be.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cl_be.Services.Implementations
{
    public class AdminProductService : IAdminProductService
    {
        private readonly AdventureWorksLt2019Context _context;

        public AdminProductService(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // get list of products with pagination
        public async Task<Page<AdminProductListDto>> GetAllProductsAsync(
            int page, 
            int pageSize,
            string? sortBy,
            string? sortDirection,
            string? search=null
        )
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .ThenInclude(p => p.ParentProductCategory)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.ProductNumber.Contains(search));
            }

            // filtering options
            query = sortBy switch
            {
                "name" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                "price" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.ListPrice)
                    : query.OrderBy(p => p.ListPrice),

                "category" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.ProductCategory!.Name)
                    : query.OrderBy(p => p.ProductCategory!.Name),

                "parentcategory" => sortDirection == "desc"
                    ? query.OrderByDescending(p =>
                        p.ProductCategory != null &&
                        p.ProductCategory.ParentProductCategory != null
                            ? p.ProductCategory.ParentProductCategory.Name : null)
                    : query.OrderBy(p =>
                        p.ProductCategory != null &&
                        p.ProductCategory.ParentProductCategory != null
                            ? p.ProductCategory.ParentProductCategory.Name : null),

                "modifieddate" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.ModifiedDate)
                    : query.OrderBy(p => p.ModifiedDate),

                _ => query.OrderBy(p => p.ProductId),
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new AdminProductListDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ProductNumber = p.ProductNumber
                })
                .ToListAsync();

            return new Page<AdminProductListDto>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<AdminProductDetailDto> GetProductDetailsAsync(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                    .ThenInclude(c => c.ParentProductCategory)
                .Include(p => p.ProductModel)
                .Where(p => p.ProductId == productId)
                .Select(p => new AdminProductDetailDto
                {
                    ProductId = p.ProductId,
                    // general:
                    ProductParentCategoryName =
                        p.ProductCategory.ParentProductCategory != null
                            ? p.ProductCategory.ParentProductCategory.Name
                            : null,
                    ProductCategoryName = p.ProductCategory.Name,
                    ProductModelName = p.ProductModel.Name,
                    ProductNumber = p.ProductNumber,
                    Name = p.Name,
                    // pricing:
                    ListPrice = p.ListPrice,
                    StandardCost = p.StandardCost,
                    // attributes:
                    Color = p.Color,
                    Size = p.Size,
                    Weight = p.Weight,
                    // availability:
                    SellStartDate = p.SellStartDate,
                    SellEndDate = p.SellEndDate,
                    DiscontinuedDate = p.DiscontinuedDate,
                })
                .FirstOrDefaultAsync();

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return product;
        }

        public async Task<AdminProductEditDto?> GetProductToEditAsync(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.SalesOrderDetails)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return null;

            return new AdminProductEditDto
            {
                // General
                ProductId = product.ProductId,
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                ProductCategoryId = product.ProductCategoryId,
                ParentCategoryId = product.ProductCategory?.ParentProductCategoryId,
                ProductModelId = product.ProductModelId,

                // Pricing
                ListPrice = product.ListPrice,
                StandardCost = product.StandardCost,

                // Attributes
                Color = product.Color,
                Size = product.Size,
                Weight = product.Weight,

                // Availability
                SellStartDate = product.SellStartDate,
                SellEndDate = product.SellEndDate,
                DiscontinuedDate = product.DiscontinuedDate,

                // Rules
                HasOrders = product.SalesOrderDetails.Any()
            };
        }


        // To get reference datas (categories and models)
        public async Task<IEnumerable<AdminProductCategoryDto>> GetCategoriesAsync()
        {
            return await _context.ProductCategories
                .AsNoTracking()
                .Select(c => new AdminProductCategoryDto
                {
                    ProductCategoryId = c.ProductCategoryId,
                    Name = c.Name,
                    ParentProductCategoryId = c.ParentProductCategoryId,
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminProductModelDto>> GetModelsAsync()
        {
            return await _context.ProductModels
                .AsNoTracking()
                .Select(m => new AdminProductModelDto
                {
                    ProductModelId = m.ProductModelId,
                    Name = m.Name
                })
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        // To update product
        public async Task UpdateProductAsync(AdminProductUpdateDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            // General
            product.Name = dto.Name.Trim();
            product.ProductNumber = dto.ProductNumber.Trim();
            product.ProductModelId = dto.ProductModelId;
            product.ProductCategoryId = dto.ProductCategoryId;

            // Pricing
            product.ListPrice = dto.ListPrice;
            product.StandardCost = dto.StandardCost;

            // Attributes
            product.Color = dto.Color?.Trim();
            product.Size = dto.Size?.Trim();
            product.Weight = dto.Weight;

            // Availability
            product.SellStartDate = dto.SellStartDate;
            product.SellEndDate = dto.SellEndDate;
            product.DiscontinuedDate = dto.DiscontinuedDate;

            product.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // CREATE/ADD NEW
        public async Task<int> CreateProductAsync(AdminProductCreateDto dto)
        {
            // validation: list price > standard cost
            if (dto.ListPrice < dto.StandardCost)
                throw new InvalidOperationException("List price cannot be lower than standard cost.");

            var product = new Product
            {
                ProductCategoryId = dto.ProductCategoryId,
                ProductModelId = dto.ProductModelId,
                ProductNumber = dto.ProductNumber,
                Name = dto.Name,

                ListPrice = dto.ListPrice,
                StandardCost = dto.StandardCost,

                Color = dto.Color,
                Size = dto.Size,
                Weight = dto.Weight,

                SellStartDate = dto.SellStartDate,
                SellEndDate = dto.SellEndDate,
                DiscontinuedDate = dto.DiscontinuedDate,

                ModifiedDate = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.ProductId;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) throw new KeyNotFoundException("Product not found");

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }
    }
}
