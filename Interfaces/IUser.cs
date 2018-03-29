
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrganizerU.Models;

namespace OrganizerU.Interfaces
{
  public interface IUser
  {
    Task<IEnumerable<Users>> Get();
    Task<Users> Get(string _id);
    Task Add(Users user);
    Task<ReplaceOneResult> Update(string _id, Users user);
    Task<DeleteResult> Remove(string _id);
  }
}