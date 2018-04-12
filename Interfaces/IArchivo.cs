using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizerU.Interfaces
{
    interface IArchivo
    {
        Task Add(string Filename, Stream stream, GridFSUploadOptions opt);
    }
}
