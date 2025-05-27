using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [Required(ErrorMessage = "O Nome é Obrigatória.")]
        public string? nome { get; set; }
        [Required(ErrorMessage = "O Email é Obrigatório.")]
        public string? email { get; set; }
        [Required(ErrorMessage = "O Endereço é Obrigatório.")]
        public string? endereco { get; set; }
    }
}
