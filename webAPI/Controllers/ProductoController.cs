using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using webAPI.Helpers;
using webAPI.Models;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {

        private readonly DbUsuarioContext _context;
        private readonly JwtService _jwtservice;
        public ProductoController(DbUsuarioContext context, JwtService jwtService)
        {
            _context = context;
            _jwtservice = jwtService;
        }

        

        [HttpPost("guardar")]
        public async Task<ActionResult<Producto>> GuardarProducto(Producto producto)
        {
            try
            {
                //validacion del token mediante cookies
                //var jwt = Request.Cookies["jwt"];
                //var token = _jwtservice.verify(jwt);

                producto.FechaCreacion = DateTime.Now;
                _context.Productos.Add(producto);

                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, producto);

            }
            catch (Exception _)
            {
                Log.Error("ERROR no autorizado");
                return Unauthorized();
            }

            
        }

        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListarProducto()
        {
            try
            {
                //validacion del token mediante cookies
                //var jwt = Request.Cookies["jwt"];
                //var token = _jwtservice.verify(jwt);

                var productos = await _context.Productos.ToListAsync();

                return Ok(productos);

            }
            catch (Exception _)
            {
                Log.Error("ERROR no autorizado");
                return Unauthorized();
            }

        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult> BuscarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);

        }
        [HttpPut("actualizar/{id}")]

        public async Task<ActionResult> ActualizarProducto(int id, Producto producto)
        {
            var productoencontrado = await _context.Productos.FindAsync(id);
            if (productoencontrado == null)
            {
                return NotFound();
            }

            productoencontrado.Codigo = producto.Codigo;
            productoencontrado.Nombre = producto.Nombre;
            productoencontrado.Tipo = producto.Tipo;
            productoencontrado.Estado = producto.Estado;
            productoencontrado.FechaCreacion = producto.FechaCreacion;

            await _context.SaveChangesAsync();
            return Ok(productoencontrado);
        }
    }
}
