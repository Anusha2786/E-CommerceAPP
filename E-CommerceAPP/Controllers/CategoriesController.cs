using E_CommerceAPP.Data;
using E_CommerceAPP.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using E_CommerceAPP.Models.Entities; // Import your entity models namespace

namespace E_CommerceAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly EcommerceDbContext categoriesdbcontext;
        private readonly ILogger<CategoriesController> _logger; // Define ILogger
        private readonly JsonSerializerOptions _jsonOptions;
        public CategoriesController(EcommerceDbContext categoriesdbcontext, ILogger<CategoriesController> logger)
        {
            this.categoriesdbcontext = categoriesdbcontext;
            _logger = logger;
        }
        //------------------------------------------------------------------------
        // GET: api/Categories
        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A list of Category objects</returns>
        /// <response code="200">Returns the list of categories</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {
            return await categoriesdbcontext.Categories.ToListAsync();
        }
        // GET: api/Categories/{id}
        /// <summary>
        /// Retrieves a specific category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve</param>
        /// <returns>A single Category object</returns>
        /// <response code="200">Returns the category with the specified ID</response>
        /// <response code="404">If no category with the specified ID exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Categories>> GetCategoryById(int id)
        {
            var category = await categoriesdbcontext.Categories
                                        .FirstOrDefaultAsync(c => c.Category_ID == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------

        // GET: api/Categories/{id}/products
        /// <summary>
        /// Retrieves products belonging to a specific category by category ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve products for</param>
        /// <returns>A list of Product objects belonging to the category</returns>
        /// <response code="200">Returns the list of products belonging to the category</response>
        /// <response code="404">If no category with the specified ID exists</response>
        [HttpGet("{categoryId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await categoriesdbcontext.Products
                .Where(p => p.Category_ID == categoryId)
                .Select(p => new ProductDTO
                {
                    Product_ID = p.Product_ID,
                    Product_Name = p.Product_Name,
                    Product_Description = p.Product_Description,
                    Product_Price = p.Product_Price,
                    Category_ID = p.Category_ID
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound();
            }

            return products;
        }
        //-------------------------------------------------
        // POST: api/Categories
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The Category object to create</param>
        /// <returns>The created Category object</returns>
        /// <response code="201">Returns the newly created category</response>
        /// <response code="400">If the request body is null or invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Categories>> PostCategory(CategoriesDTO categoryWithProducts)
        {
            // Validate the incoming DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new category entity
            var newCategory = new Categories
            {
                Category_Name = categoryWithProducts.Category_Name,
                Products = new List<Products>() // Initialize Products collection
            };

            // Add products to the new category
            foreach (var productDTO in categoryWithProducts.Products)
            {
                var product = new Products
                {
                    Product_Name = productDTO.Product_Name,
                    Product_Description = productDTO.Product_Description,
                    Product_Price = productDTO.Product_Price,
                    Category_ID = newCategory.Category_ID // Assign Category_ID to product
                };

                newCategory.Products.Add(product);
            }

            // Save changes to the database
            categoriesdbcontext.Categories.Add(newCategory);
            await categoriesdbcontext.SaveChangesAsync();

            // Return a response with status 201 (Created)
            // Include the newly created category in the response
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Category_ID }, newCategory);
        }


        //------------------------------------------------------------------------------------------------------------------------
        // PUT: api/Categories/{id}
        /// <summary>
        /// Updates an existing category identified by ID.
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="category">The updated Category object</param>
        /// <returns>The updated Category object</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="400">If the request body or ID is invalid</response>
        /// <response code="404">If no category with the specified ID exists</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, UpdateonlyCategoryDTO updatedCategory)
        {
            var categoryToUpdate = await categoriesdbcontext.Categories.FindAsync(id);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            // Update only the category name
            categoryToUpdate.Category_Name = updatedCategory.Category_Name;

            try
            {
                await categoriesdbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //-------------------------------------------------------------

        // PUT: api/Categories/update-with-products/{id}
        /// <summary>
        /// Updates an existing category and its associated products identified by ID.
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <response code="200">Returns the updated category with its associated products</response>
        /// <response code="400">If the request body or ID is invalid</response>
        /// <response code="404">If no category with the specified ID exists</response>
        [HttpPut("update-with-products/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, UpdatecategoryDTO updatedCategory)
        {
            // Retrieve existing category including products
            var categoryToUpdate = await categoriesdbcontext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Category_ID == id);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            // Update category details
            categoryToUpdate.Category_Name = updatedCategory.Category_Name;

            // Update or add products
            foreach (var updatedProduct in updatedCategory.Products)
            {
                var existingProduct = categoryToUpdate.Products.FirstOrDefault(p => p.Product_ID == updatedProduct.Product_ID);
                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.Product_Name = updatedProduct.Product_Name;
                    existingProduct.Product_Description = updatedProduct.Product_Description;
                    existingProduct.Product_Price = updatedProduct.Product_Price;
                }
                else
                {
                    // Add new product to the category (if necessary)
                    var newProduct = new Products
                    {
                        Product_Name = updatedProduct.Product_Name,
                        Product_Description = updatedProduct.Product_Description,
                        Product_Price = updatedProduct.Product_Price,
                        Category_ID = id // Assign category ID to link product with category
                    };
                    categoryToUpdate.Products.Add(newProduct);
                }
            }

            // Remove products that are not in updatedCategory.Products
            var productsToRemove = categoryToUpdate.Products.Where(p => !updatedCategory.Products.Any(up => up.Product_ID == p.Product_ID)).ToList();
            foreach (var productToRemove in productsToRemove)
            {
                categoriesdbcontext.Products.Remove(productToRemove);
            }

            try
            {
                await categoriesdbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //---------------------------------------------------------------------------


        private bool CategoryExists(int id)
        {
            return categoriesdbcontext.Categories.Any(e => e.Category_ID == id);
        }
        // DELETE: api/Categories/{id}
        /// <summary>
        /// Deletes a category identified by ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the category was successfully deleted</response>
        /// <response code="404">If no category with the specified ID exists</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await categoriesdbcontext.Categories
                                        .Include(c => c.Products) // Include products for deletion
                                        .FirstOrDefaultAsync(c => c.Category_ID == id);

            if (category == null)
            {
                return NotFound();
            }

            try
            {
                // Remove all associated products
                categoriesdbcontext.Products.RemoveRange(category.Products);

                // Remove the category itself
                categoriesdbcontext.Categories.Remove(category);

                await categoriesdbcontext.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the category." });
            }
        }
    }

}
