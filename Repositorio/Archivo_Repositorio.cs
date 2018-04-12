using Microsoft.Extensions.Options;
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

        public async Task Add(string Filename, Stream stream, GridFSUploadOptions opt) => await context.Grid.UploadFromStreamAsync(Filename, stream, opt);
    }
}
