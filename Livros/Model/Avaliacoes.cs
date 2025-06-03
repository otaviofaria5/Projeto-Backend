using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Avaliacoes
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string? Id { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("livroId")]
        public string? LivroId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("usuarioId")]
        public string? UsuarioId { get; set; }
        [Required(ErrorMessage = "A Avalição é obrigatória.")]
        [BsonElement("avaliacao")]
        public int? Avaliacao { get; set; } // 1 a 5
        [Required(ErrorMessage = "O comentário é obrigatório.")]
        [BsonElement("comentario")]
        public string? Comentario { get; set; }


    }
}
