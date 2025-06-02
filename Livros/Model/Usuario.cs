using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string? Id { get; set; }
        [Required(ErrorMessage = "O Nome é Obrigatória.")]
        [BsonElement("nome")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "O Email é Obrigatório.")]
        [BsonElement("email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "O Endereço é Obrigatório.")]
        [BsonElement("endereco")]
        public string? Endereco { get; set; }
    }
}
