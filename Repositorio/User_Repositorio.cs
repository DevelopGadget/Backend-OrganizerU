using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Repositorio
{

  public class User_Repositorio : IUser
  {
    private readonly ObjectContext context = null;
    public User_Repositorio(IOptions<Settings> settings)
    {
      context = new ObjectContext(settings);
    }
    public async Task Add(User user)
    {
      await context.User.InsertOneAsync(user);
    }
    public async Task<IEnumerable<User>> Get()
    {
      return await context.User.Find(x => true).ToListAsync();
    }
    public async Task<User> Get(string _id)
    {
      return await context.User.Find(Builders<User>.Filter.Eq("Id", _id)).FirstOrDefaultAsync();
    }
    public async Task<DeleteResult> Remove(string _id)
    {
      return await context.User.DeleteOneAsync(Builders<User>.Filter.Eq("Id", _id));
    }
    public async Task<ReplaceOneResult> Update(string _id, User user)
    {
      return await context.User.ReplaceOneAsync(o => o.Id.Equals(_id), user);
    }
  }

}