
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrganizerU.Models;

namespace OrganizerU.Interfaces
{
  public interface IUser
  {
    Task<IEnumerable<User>> Get();
    Task<User> Get(string _id);
    Task Add(User user);
    Task<ReplaceOneResult> Update(string _id, User user);
    Task<DeleteResult> Remove(string _id);
  }
}