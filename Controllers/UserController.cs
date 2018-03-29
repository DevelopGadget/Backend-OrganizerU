using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrganizerU.Models;

namespace OrganizerU.Controllers
{
  [Route("[controller]")]
  public class User : Controller
  {
    private IConfiguration _config;

    public User(IConfiguration config)
    {
      _config = config;
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
    private void Authenticate(User login)
    {
      User user = null;
      if (login.Username == "mario" && login.Password == "secret")
      {
      }

    }

    private string BuildToken(UserModel user)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
