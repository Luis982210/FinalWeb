using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data.SqlClient;

namespace Front.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration config;

        public IActionResult Index()
        {
            return View();
        }
        public LoginController(IConfiguration config)
        {
            this.config = config; 
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(string email, string password)
        {
            SqlConnection con = new SqlConnection("Server=LUIS\\MSSQLSERVER01; Database=Bicicletas; Integrated Security=sspi;trusted_connection=true;TrustServerCertificate=True");
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Usuarios WHERE Correo='" + email + "'AND Contraseña = '" + password+"'" , con);
            DataTable dt = new DataTable("contador");
            Console.WriteLine(email);
            Console.WriteLine(password);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                Console.WriteLine("ENCONTRADO");
                var claims = new List<Claim>();
                claims.Add(new Claim("password", password));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, "1234"));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                return RedirectToAction("Index", "Bicicletas");
            }
            else
            {
                
               
                    ViewBag.Email = "Ingresa tus credenciales nuevamente";
                    Console.WriteLine("NO ENCONTRADO");
                    return Redirect("/Login");

                
            }



        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "login");
        }



        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)

        {

            var _symmetricSecurityKey = new SymmetricSecurityKey(

                    Encoding.UTF8.GetBytes(config["JWT:SecretKey"])

                );

            var _signingCredentials = new SigningCredentials(

                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256

                );

            var _Header = new JwtHeader(_signingCredentials);

            var _Claims = new[] {

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(JwtRegisteredClaimNames.NameId, ApplicationName)

            };

            var _Payload = new JwtPayload(

                    issuer: config["JWT:Issuer"],

                    audience: config["JWT:Audience"],

                    claims: _Claims,

                    notBefore: DateTime.UtcNow,

                    expires: token_expiration

                );

            var _Token = new JwtSecurityToken(

                    _Header,

                    _Payload

                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);

        }
    }
}
