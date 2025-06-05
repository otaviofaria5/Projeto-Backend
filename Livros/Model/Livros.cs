using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Livros
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string? Id { get; set; }

        [Required(ErrorMessage = "O Titulo é obrigatório.")]
        [BsonElement("titulo")]
        public string? Titulo { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("autorId")]
        public string? AutorId { get; set; }

        [Required(ErrorMessage = "O Genero é obrigatória.")]
        [BsonElement("genero")]
        public string? Genero { get; set; }

        [Required(ErrorMessage = "O Ibsn é obrigatória.")]
        [BsonElement("isbn")]
        public string? Isbn { get; set; }
        [Required(ErrorMessage = "A Descricão é obrigatória.")]
        [BsonElement("descricao")]
        public string? Descricao { get; set; }

        [BsonDefaultValue(true)]
        public bool Disponivel { get; set; } = true;

    }
}
