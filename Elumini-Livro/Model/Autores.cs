using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Autores
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [Required(ErrorMessage = "O Nome é Obrigatório.")]
        public string? nome { get; set; }
        [Required(ErrorMessage = "A Biografia é Obrigatória.")]
        public string? biografia { get; set; }
        [Required(ErrorMessage = "A Nacionalidade é Obrigatória.")]
        public string? nacionalidade { get; set; }

    }
}
