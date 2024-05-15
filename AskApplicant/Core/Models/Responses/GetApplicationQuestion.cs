using AskApplicant.Core.DTOs;
using AskApplicant.Core.Entities;
using AskApplicant.Core.Enums;
using MongoDB.Bson;

namespace AskApplicant.Core.Models.Responses
{
    public class GetApplicationFormQuestions
    {
        public string ProgramInfoId { get; set; }
        public ProgramInfoDto? ProgramInfo { get; set; }
        public List<QuestionWithMultiChoice> Questions { get; set; }
    }

    public class QuestionWithMultiChoice
    {
        public string Id { get; set; }
        public string Quesstion { get; set; }
        public QuestionType QuestionType { get; set; }
        public int ChoiceLimit { get; set; } = 0;
        public bool IsMultiChoice { get; set; } = false;
        public string ProgramInfoId { get; set; }
        public List<MultiChoiceResponse> MultiChoices { get; set; }
    }
}
