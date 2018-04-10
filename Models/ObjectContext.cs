using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OrganizerU.Models {
  public class ObjectContext {
    public IConfiguration Configuration { get; set; }
    private IMongoDatabase _database = null;
    public ObjectContext (IOptions<Settings> Setting) {
      Configuration = Setting.Value.configuration;
      Setting.Value.ConectionString = Configuration["ConectionMlab"].ToString();
      Setting.Value.Database = Configuration["Database"].ToString();
      var Client = new MongoClient (Setting.Value.ConectionString);
      if (Client != null) { _database = Client.GetDatabase (Setting.Value.Database); }
    }
    public IMongoCollection<Users> User {
      get { return _database.GetCollection<Users> ("Usuarios"); }
    }

    public IMongoCollection<Estudiante> Estudiante {
      get { return _database.GetCollection<Estudiante> ("Estudiantes"); }
    }
  }
}