using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [BsonIgnoreExtraElements] 
    public class Autores
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string? Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [BsonElement("nome")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "A Biografia é obrigatória.")]
        [BsonElement("biografia")]
        public string? Biografia { get; set; }

        [Required(ErrorMessage = "A Nacionalidade é obrigatória.")]
        [BsonElement("nacionalidade")]
        public string? Nacionalidade { get; set; }
    }
}
