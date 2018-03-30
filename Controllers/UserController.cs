using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Controllers
{
  [Route("[controller]")]
  public class UserController : Controller
  {
    private IConfiguration _config;
    private readonly IUser _user;
    public UserController(IConfiguration config, IUser user)
    {
      this._config = config;
      this._user = user;
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    [AllowAnonymous]
    [HttpPost("Crear")]
    public async Task<IActionResult> CrearCuenta([FromBody]Users user)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      else
      {
        if (await isUniqueAsync(user: user))
        {
          await _user.Add(user);
          return BuildToken(user);
        }
        else
        {
          return BadRequest("Ya existe esa cuenta");
        }
      }
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    private async Task<bool> Authenticate(Users login)
    {
      if (login == null)
      {
        return false;
      }
      foreach (Users us in await _user.Get())
      {
        if (us.Username.Equals(login.Username) && us.Username.Equals(login.Password)) return true;
      }
      return false;
    }
    private async Task<bool> isUniqueAsync(Users user)
    {
      if (user == null)
      {
        return false;
      }
      foreach (Users us in await _user.Get())
      {
        if (us.Username.Equals(user.Username)) return false;
      }
      return true;
    }
    private IActionResult BuildToken(Users user)
    {
      var claims = new[]
      {
          new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
          new Claim("miValor", "Lo que yo quiera"),
          new Claim(JwtRegisteredClaimNames.Jti, System.Guid.NewGuid().ToString())
      };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var expiration = System.DateTime.Now.AddMinutes(30);
      JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        claims: claims,
        expires: expiration,
        signingCredentials: creds);
      return Ok(new
      {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiration = expiration
      });
    }
  }
}
