using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Projeto_Backend.Model
{
    public class Livros
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Titulo { get; set; }

        public string? AutorId { get; set; }

        public string Genero { get; set; }

        public string isbn { get; set; }

        public string descricao { get; set; }


    }
}
