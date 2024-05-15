using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AskApplicant.Core.Models.Responses
{
    public class MultiChoiceResponse
    {
      
        public string Id { get; set; }
        public string Choice { get; set; }
        public string ProgramInfoId { get; set; }
        public string QuestionId { get; set; }
    }
}
