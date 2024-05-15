using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using AskApplicant.Core.Enums;

namespace AskApplicant.Core.Entities
{
    public class ApplicantAnswer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Answer { get; set; }
        public ObjectId ProgramInfoId { get; set; }
        public ObjectId QuestionId { get; set; }
    }
}
