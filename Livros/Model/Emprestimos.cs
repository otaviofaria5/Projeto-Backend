using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Model
{
    public class Emprestimos
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

        public DateTimeOffset? dataEmprestimo { get; set; }
        public DateTimeOffset? dataDevolucaoPrevista { get; set; }
       public DateTimeOffset? dataDevolucaoReal {  get; set; }
    }
}
