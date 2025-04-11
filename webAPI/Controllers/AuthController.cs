using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webAPI.DTO;
using webAPI.Helpers;
using webAPI.Models;

namespace webAPI.Controllers
{

   

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly DbUsuarioContext _context;
        private readonly JwtService _jwtservice;
        public AuthController(DbUsuarioContext _contextUsu, JwtService jwtService)
        {
            _context = _contextUsu;
            _jwtservice = jwtService;
        }



        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(LoginDTO dto)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(Usuario => Usuario.Username == dto.Username);
            if (user == null) {
                return BadRequest(error: new { message = "Invalid Credentials" });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password,user.Password))
            {
                return BadRequest(error: new { message = "Invalid Credentials" });
            }

            var jwt = _jwtservice.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(
                new
                {
                    message = "success",
                    UserId = user.Id
                }
                );

        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogOut()
        {
            Response.Cookies.Delete("jwt");
            return Ok(
                new
                {
                    message = "success"
                }
                );
        }
    }
}
