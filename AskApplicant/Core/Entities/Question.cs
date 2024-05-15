using AskApplicant.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AskApplicant.Core.Entities
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Quesstion { get; set; }
        public QuestionType QuestionType { get; set; }
        public int ChoiceLimit { get; set; } = 0;
        public bool IsMultiChoice { get; set; } = false;
        public ObjectId ProgramInfoId { get; set; }
    }

    public class MultiChoice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Choice { get; set; }
        public ObjectId ProgramInfoId { get; set; }
        public ObjectId QuestionId { get; set; }
    }
}
