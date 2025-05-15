using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Projeto_Backend.Model
{
    public class Autores
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? nome { get; set; }

        public string? biografia { get; set; }

        public string? nacionalidade { get; set; }

    }
}
