using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Controllers
{
  [Route("[controller]")]
  public class User : Controller
  {
    private IConfiguration _config;
    private readonly IUser _user;
    public User(IConfiguration config, IUser user)
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

    // POST api/values
    [HttpPost("Crear")]
    public void Post([FromBody]User User)
    {
      if (!ModelState.IsValid)
      {
        BadRequest();
      }
      else
      {

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
    private string BuildToken(User user, IConfiguration config)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(config["Jwt:Issuer"],
        config["Jwt:Issuer"],
        expires: System.DateTime.Now.AddMinutes(30),
        signingCredentials: creds);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
