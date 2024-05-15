using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using MongoDB.Bson;

namespace AskApplicant.Core.Application.Implementation
{
    public interface IApplicantService
    {
        Task<BaseResponse<bool>> FillApplicationForm(FillApplicationFormRequest forms);

        Task<BaseResponse<GetApplicationFormQuestions>> GetApplicationFormQuestionsByProgramInfoId(ObjectId Id);
    }
}
