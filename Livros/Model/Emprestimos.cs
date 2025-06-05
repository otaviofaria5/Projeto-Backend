using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Model
{
    [BsonIgnoreExtraElements]
    public class Emprestimos
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

        [BsonElement("dataEmprestimo")]
        public DateTimeOffset? DataEmprestimo { get; set; }
        [BsonElement("dataDevolucaoPrevista")]
        public DateTimeOffset? DataDevolucaoPrevista { get; set; }
        [BsonElement("dataDevolucaoReal")]
        public DateTimeOffset? DataDevolucaoReal {  get; set; }
        [BsonElement("devolvido")]
        public bool Devolvido { get; set; }
    }
}
