using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using webAPI.Helpers;
using webAPI.Models;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbUsuarioContext _dbUsuarioContext;
        private readonly JwtService _jwtservice;
        public UsuarioController(DbUsuarioContext dbUsuarioContext,  JwtService jwtService)
        {
            _dbUsuarioContext = dbUsuarioContext;
            _jwtservice = jwtService;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListarUsuario()
        {
            try
            {
                //validacion del token mediante cookies
                var jwt = Request.Cookies["jwt"];
                var token = _jwtservice.verify(jwt);
                //int userId = int.Parse(token.Issuer);

                var usuarios = await _dbUsuarioContext.Usuarios.ToListAsync();
                
                return Ok(usuarios);
                
            }
            catch (Exception _) {
                Log.Error("ERROR no autorizado");
                return Unauthorized();
            }
            
        }

        [HttpPost("guardar")]
        public async Task<ActionResult<Usuario>> GuardarUsuario(Usuario usuario)
        {
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
           usuario.FechaCreacion = DateTime.Now;
        _dbUsuarioContext.Usuarios.Add(usuario);
            await _dbUsuarioContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, usuario);
        }

        [HttpPut("actualizar/{id}")]
        
        public async Task<ActionResult> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuariolist = await _dbUsuarioContext.Usuarios.FindAsync(id);
            if(usuariolist == null)
            {
                return NotFound();
            }

            usuariolist.Nombres = usuario.Nombres;
            usuariolist.Apellidos = usuario.Apellidos;
            usuariolist.Correo = usuario.Correo;
            usuariolist.Username = usuario.Username;

            await _dbUsuarioContext.SaveChangesAsync();
            return Ok(usuariolist);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var usuariolist = await _dbUsuarioContext.Usuarios.FindAsync(id);
            if (usuariolist == null)
            {
                return NotFound();
            }

            _dbUsuarioContext.Usuarios.Remove(usuariolist);
            await _dbUsuarioContext.SaveChangesAsync();
            return NoContent();

        }

       

    }
}
