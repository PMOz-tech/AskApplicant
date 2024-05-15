using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using MongoDB.Bson;

namespace AskApplicant.Core.Application.Implementation
{
    public interface IEmployerService
    {
        Task<BaseResponse<string>> CreateApplicationForm(CreateApplicationForm applicationForm);

        Task<BaseResponse<bool>> EditApplicationForm(List<EditApplicationForm> forms);

        Task<BaseResponse<GetApplicationFormQuestions>> GetApplicationFormQuestionsByProgramInfoId(ObjectId Id);
    }


}
