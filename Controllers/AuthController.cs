
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using test.Model;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace YourProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // Kiểm tra thông tin đăng nhập
                bool isValid = ValidateLogin(request.id, request.emPassword);
                if (!isValid)
                {
                    return Unauthorized("Invalid username or password");
                }

                // Tạo và trả về JWT
                var token = GenerateJwtToken(request.id);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    

        private bool ValidateLogin(int id, string password)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblEmployee WHERE ID=@ID AND EmPassword=@EmPassword", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@EmPassword", password);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        // Hàm tạo JWT Token
        private string GenerateJwtToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }
    }

}





