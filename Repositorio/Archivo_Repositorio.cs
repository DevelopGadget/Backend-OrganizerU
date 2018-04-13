using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using OrganizerU.Interfaces;
using OrganizerU.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizerU.Repositorio
{
    public class Archivo_Repositorio : IArchivo
    {
        private readonly ObjectContext context = null;
        public Archivo_Repositorio(IOptions<Settings> settings) => context = new ObjectContext(settings);

        public async Task<ObjectId> Add(string Filename, Stream stream, GridFSUploadOptions opt) => await context.Grid.UploadFromStreamAsync(Filename, stream, opt);

        public async Task Delete(ObjectId Id) => await context.Grid.DeleteAsync(Id);

        public async Task<GridFSFileInfo> Get(ObjectId Id) => context.Grid.Find(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, "fernando.docx")).ToList().FirstOrDefault();
    }
}
