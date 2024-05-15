using AskApplicant.Core.Enums;
using MongoDB.Bson;

namespace AskApplicant.Core.DTOs
{
    public class QuestionDto
    {
        public QuestionType QuestionType { get; set; }
        public string Quesstion { get; set; }
        public List<MultiChoiceDto> Choices { get; set; }
        public int? ChoiceLimit { get; set; } = 0;
    }

    public class MultiChoiceDto
    {
        public string Choice { get; set; }

    }

}
