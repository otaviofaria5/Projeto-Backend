using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "O Genero é obrigatória.")]
        public string? genero { get; set; }

        [Required(ErrorMessage = "O Ibsn é obrigatória.")]
        public string? isbn { get; set; }
        [Required(ErrorMessage = "A Descricão é obrigatória.")]

        public string? descricao { get; set; }

        [BsonDefaultValue(true)]
        public bool Disponivel { get; set; } = true;

    }
}
