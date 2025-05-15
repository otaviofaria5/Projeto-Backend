using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Projeto_Backend.Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? endereco { get; set; }
    }
}
