using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizerU.Interfaces
{
    public interface IArchivo
    {
        Task<GridFSFileInfo> Get(ObjectId Id);
        Task<ObjectId> Add(string Filename, Stream stream, GridFSUploadOptions opt);
        Task Delete(ObjectId Id);
        Task Rename(ObjectId Id, string FileName);
    }
}
