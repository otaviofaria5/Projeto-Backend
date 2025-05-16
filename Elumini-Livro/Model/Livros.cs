using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Livros
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        public string? titulo { get; set; }
        [BsonElement("autorId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? autorId { get; set; }

        public string? genero { get; set; }

        public string? isbn { get; set; }

        public string? descricao { get; set; }


    }
}
