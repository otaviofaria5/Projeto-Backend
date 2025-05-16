using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Model
{
    public class Avaliacoes
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("livroId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? livroId { get; set; }
        [BsonElement("usuarioId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? usuarioId { get; set; }

        public int? avaliacao { get; set; } // 1 a 5
        public string? comentario { get; set; }

        #pragma warning disable IDE1006

    }
}
