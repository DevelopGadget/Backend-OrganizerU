using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Controllers {
  [Route ("[controller]")]
  public class UserController : Controller {
    private IConfiguration _config;
    private readonly IUser _user;
    private readonly IEstudiante _estudiante;
    public UserController (IConfiguration config, IUser user, IEstudiante _estudiante) {
      this._config = config;
      this._user = user;
      this._estudiante = _estudiante;
    }

    [Authorize]
    [HttpGet ("{id}")]
    public Task<IActionResult> Get (string id) {
      return GetUser (id);
    }
    private async Task<IActionResult> GetUser (string id) {
      try {
        if (id.Length < 24) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
        }
        Users us = await _user.Get (id);
        if (us == null) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
        }
        return Ok (JsonConvert.SerializeObject (us));
      } catch (Exception) {
        return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
      }
    }

    [AllowAnonymous]
    [HttpPost ("Crear")]
    public async Task<IActionResult> CrearCuenta ([FromBody] Users user) {
      try {
        if (!ModelState.IsValid) {
          return BadRequest (ModelState);
        } else {
          if (await isUniqueAsync (user: user)) {
            await _user.Add (user);
            await _estudiante.Add (new Estudiante (IdGet (user).Result));
            return Ok ("Creado");
          } else {
            return StatusCode (StatusCodes.Status406NotAcceptable,"Ya existe esa cuenta");
          }
        }
      } catch (Exception) {
        return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
      }
    }

    [AllowAnonymous]
    [HttpPost ("Login")]
    public async Task<IActionResult> Login ([FromBody] Users user) {
      try {
        if (!ModelState.IsValid) {
          return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
        } else {
          if (await Authenticate (user)) {
            return BuildToken (await _estudiante.Get (IdGet (user).Result), user);
          } else {
            return Unauthorized ();
          }
        }
      } catch (Exception e) {
        return BadRequest (e);
      }
    }

    [Authorize]
    [HttpPut ("{id}")]
    public async Task<IActionResult> Put (string id, [FromBody] Users user) {
      try {
        if (string.IsNullOrEmpty (id) || id.Length < 24) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"Id Invalid");
        }
        if (await _user.Get (id) == null) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"No ha coincidencias");
        }
        if (ModelState.IsValid) {
          user.Id = id;
          var h = await _user.Update (id, user);
          if (h.MatchedCount > 0) {
            return Ok ("Modificado");
          } else {
            return BadRequest ("Hubo un error");
          }
        } else {
          return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
        }
      } catch (Exception) {
        return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
      }
    }

    [Authorize]
    [HttpDelete ("{id}")]
    public async Task<IActionResult> Delete (string id) {
      try {
        if (string.IsNullOrEmpty (id) || id.Length < 24) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"Id invalid");
        }
        if (await _user.Get (id) == null) {
          return StatusCode (StatusCodes.Status406NotAcceptable,"Invalid Id");
        }
        var h = await _user.Remove (id);
        var e = await _estudiante.Remove (id);
        if (h.DeletedCount > 0 && e.DeletedCount > 0) {
          return Ok ("Eliminado");
        } else {
          return BadRequest ("Hubo un error");
        }
      } catch (Exception) {
        return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
      }
    }
    private async Task<bool> Authenticate (Users login) {
      if (login == null) {
        return false;
      }
      foreach (Users us in await _user.Get ()) {
        if (us.Username.Equals (login.Username) && us.Password.Equals (login.Password)) return true;
      }
      return false;
    }
    private async Task<bool> isUniqueAsync (Users user) {
      if (user == null) {
        return false;
      }
      foreach (Users us in await _user.Get ()) {
        if (us.Username.Equals (user.Username)) return false;
      }
      return true;
    }
    private async Task<string> IdGet (Users user) {
      if (user == null) {
        return null;
      }
      foreach (Users us in await _user.Get ()) {
        if (us.Username.Equals (user.Username)) return us.Id;
      }
      return null;
    }
    private IActionResult BuildToken (Estudiante estudiante, Users user) {
      var claims = new [] {
        new Claim (JwtRegisteredClaimNames.UniqueName, user.Username),
        new Claim ("Role", "User"),
        new Claim ("Datos", JsonConvert.SerializeObject (estudiante)),
        new Claim (JwtRegisteredClaimNames.Jti, System.Guid.NewGuid ().ToString ())
      };
      var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config["Key"]));
      var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);
      var expiration = System.DateTime.Now.AddMinutes (30);
      JwtSecurityToken token = new JwtSecurityToken (_config["Issuer"],
        _config["Issuer"],
        claims : claims,
        expires : expiration,
        signingCredentials : creds);
      return Ok (new {
        token = new JwtSecurityTokenHandler ().WriteToken (token),
          expiration = expiration
      });
    }
  }
}