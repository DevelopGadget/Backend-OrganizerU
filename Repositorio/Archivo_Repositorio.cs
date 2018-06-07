using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using OrganizerU.Interfaces;
using OrganizerU.Models;
using System.IO;
using System.Threading.Tasks;

namespace OrganizerU.Repositorio
{
    public class Archivo_Repositorio : IArchivo
    {
        private readonly ObjectContext context = null;
        public Archivo_Repositorio(IOptions<Settings> settings) => context = new ObjectContext(settings);

        public async Task<ObjectId> Add(string Filename, Stream stream, GridFSUploadOptions opt) => await context.Grid.UploadFromStreamAsync(Filename, stream, opt);

        public async Task Delete(string Id) => await context.Grid.DeleteAsync(Id);

        public async Task<GridFSFileInfo> Get(string Id) => await  context.Grid.Find(Builders<GridFSFileInfo>.Filter.Eq<string>(info => info.Id.ToString(), Id)).FirstOrDefaultAsync();

        public async Task Rename(string Id, string FileName) => await context.Grid.RenameAsync(Id, FileName);
    }
}
