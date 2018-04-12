using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace OrganizerU.Models
{
    public class ObjectContext
    {
        public IConfiguration Configuration { get; set; }
        private IMongoDatabase _database = null;
        private GridFSBucketOptions op = new GridFSBucketOptions();
        public ObjectContext(IOptions<Settings> Setting)
        {
            Configuration = Setting.Value.configuration;
            Setting.Value.ConectionString = Configuration["ConectionMlab"].ToString();
            Setting.Value.Database = Configuration["Database"].ToString();
            var Client = new MongoClient(Setting.Value.ConectionString);
            if (Client != null) { _database = Client.GetDatabase(Setting.Value.Database); op.BucketName = "Archivos"; }
        }

        public IMongoCollection<Users> User
        {
            get { return _database.GetCollection<Users>("Usuarios"); }
        }

        public IMongoCollection<Estudiante> Estudiante
        {
            get { return _database.GetCollection<Estudiante>("Estudiantes"); }
        }

        public GridFSBucket Grid
        {
            get { return new GridFSBucket(_database, op); }
        }
    }
}