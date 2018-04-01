using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrganizerU.Models {
  public class Users {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    public Users (string Username, string Password) {
      this.Username = Username;
      this.Password = Password;
    }
  }
}