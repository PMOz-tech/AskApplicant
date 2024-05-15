using AskApplicant.Core.DTOs;
using MongoDB.Bson;

namespace AskApplicant.Core.Models.Requests
{
    public class FillApplicationFormRequest
    {
        public string ProgramId { get; set; }
        public ContactInformationDto ContactInformation { get; set;}
        public List<AnsweredQuestions> AnsweredQuestions { get; set; }
    }

    public class AnsweredQuestions
    {
        public ObjectId QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
