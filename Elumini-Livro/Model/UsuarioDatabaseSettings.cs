using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Projeto_Backend.Model
{
    public class UsuarioDatabaseSettings
    {

        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? CollectionName { get; set; }
    }
}
