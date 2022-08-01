using APIRestProductManagement.Entities;
using APIRestProductManagement.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using APIRestProductManagement.Utils;

namespace APIRestProductManagement.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ProductsController(ApplicationDbContext contex, IMapper mapper)
        {
            this.context = contex;
            this.mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                return mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpGet("get-all-pager")]
        public async Task<ActionResult<PaginadorGenerico<Product>>> GetPager(string search, int pagina = 1, int recordPerPage = 10)
        {
            try
            {

                List<Product> products;
                PaginadorGenerico<Product> paginadorProducts;

                products = await context.Products.ToListAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    foreach (var item in search.Split(new char[] { ' ' },
                             StringSplitOptions.RemoveEmptyEntries))
                    {
                        products = products.Where(x => x.Description.Contains(item) ||
                                                      x.SupplierDescription.Contains(item))
                                                      .ToList();
                    }
                }

                int _TotalRegistros = 0;
                int _TotalPaginas = 0;

                _TotalRegistros = products.Count();

                products = products.Skip((pagina - 1) * recordPerPage)
                                                  .Take(recordPerPage)
                                                  .ToList();

                _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / recordPerPage);

                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                paginadorProducts = new PaginadorGenerico<Product>()
                {
                    RegistrosPorPagina = recordPerPage,
                    TotalRegistros = _TotalRegistros,
                    TotalPaginas = _TotalPaginas,
                    PaginaActual = pagina,
                    BusquedaActual = search,
                    Resultado = products
                };

                return paginadorProducts;

                //return mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-by-id/{id:Guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            try
            {
                var exists = await context.Products.AnyAsync(x => x.Id == id);

                if (!exists)
                {
                    return BadRequest($"No existe el producto con ID: {id}");
                }

                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
                return mapper.Map<ProductDto>(product);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{description}/{supplier}")]
        public async Task<ActionResult<List<ProductDto>>> GetByDescription(string description, string supplier)
        {
            try
            {
                var products = await context.Products.Where(x => x.Description.Contains(description) && x.SupplierDescription.Contains(supplier)).ToListAsync();
                return mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddProduct(ProductDto productModel)
        {
            try
            {
                if (productModel.ManufacturingDate >= productModel.ValidityDate)
                {
                    return BadRequest("La fecha de fabricación no puede ser mayor a la fecha de vencimiento.");
                }

                var product = mapper.Map<Product>(productModel);

                context.Add(product);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [HttpPut("UpdateProduct/{id:Guid}")]
        public async Task<ActionResult> UpdateProduct(ProductDto productModel, Guid id)
        {
            if(productModel.Id != id)
            {
                return BadRequest("El id del producto no coincide con el de la URL");
            }
            else if (productModel.ManufacturingDate >= productModel.ValidityDate)
            {
                return BadRequest("La fecha de fabricación no puede ser mayor a la fecha de vencimiento.");
            }

            try
            {
                var exists = await context.Products.AnyAsync(x => x.Id == id);

                if (!exists)
                {
                    return BadRequest($"No existe el producto con ID: {id}");
                }

                var product = mapper.Map<Product>(productModel);
                context.Update(product);
                await context.SaveChangesAsync();
                return Ok("Producto actualizado exitosamente");
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpPatch("UpdateStatus/{id:guid}")]
        public async Task<ActionResult> UpdateStatus(Guid id, JsonPatchDocument<DisableEnableProductoDto> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest();                    
                }

                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    return NotFound("Libro no encontrado");
                }

                var productDto = mapper.Map<DisableEnableProductoDto>(product);

                patchDocument.ApplyTo(productDto, ModelState);

                var isValid = TryValidateModel(productDto);

                if (!isValid)
                {
                    return BadRequest("Modelo inválido. Sólo puede cambiar el status del producto");
                }

                mapper.Map(productDto, product);
                await context.SaveChangesAsync();

                return Ok("Producto actualizado exitosamente");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
