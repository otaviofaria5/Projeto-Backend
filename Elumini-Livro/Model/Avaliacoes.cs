using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "A Avalição é obrigatória.")]
        public int? avaliacao { get; set; } // 1 a 5
        [Required(ErrorMessage = "O comentário é obrigatório.")]
        public string? comentario { get; set; }

        #pragma warning disable IDE1006

    }
}
