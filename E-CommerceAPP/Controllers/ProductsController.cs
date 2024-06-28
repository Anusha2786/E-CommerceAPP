using E_CommerceAPP.Data;
using E_CommerceAPP.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_CommerceAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EcommerceDbContext productsdbcontext;
        private readonly JsonSerializerOptions _jsonOptions;
        public ProductsController(EcommerceDbContext productsdbcontext)
        {
            this.productsdbcontext = productsdbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetCategories()
        {
            return await productsdbcontext.Products.ToListAsync();
        }

    }
}

