using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Projeto_Backend.Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
    }
}
