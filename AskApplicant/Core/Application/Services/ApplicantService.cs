using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.Entities;
using AskApplicant.Core.Enums;
using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using AskApplicant.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AskApplicant.Core.Application.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly AskApplicantDbContext _dbContext;
        public ApplicantService(AskApplicantDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<BaseResponse<bool>> FillApplicationForm(FillApplicationFormRequest forms)
        {
            try
            {
                ObjectId programId;
                if (!ObjectId.TryParse(forms.ProgramId, out programId))
                {
                    return new BaseResponse<bool>(false, "Invalid ProgramId ");
                }
                var getApplicationForm = _dbContext.ProgramInfos.Find(q => q.Id == programId).FirstOrDefaultAsync();

                if (getApplicationForm is null) return new BaseResponse<bool>(false, "Application form does not exist");

                if (!string.IsNullOrEmpty(forms.ContactInformation.PhoneNumber) && forms.ContactInformation.PhoneNumber.StartsWith("+")) return new BaseResponse<bool>(false, "Phone number does not start with country code");

                var addContactInformation = new ContactInformation
                {
                    IdentificationNumber = forms.ContactInformation.IdentificationNumber,
                    ProgramInfoId = programId,
                    Nationality = forms.ContactInformation.Nationality,
                    PhoneNumber = forms.ContactInformation.PhoneNumber,
                    Dob = forms.ContactInformation.Dob,
                    FirstName = forms.ContactInformation.FirstName,
                    LastName = forms.ContactInformation.LastName,
                    Email = forms.ContactInformation.Email,
                    Gender = forms.ContactInformation.Gender.ToString(),
                    Residence = forms.ContactInformation.Residence
                };

                await _dbContext.ContactInformations.InsertOneAsync(addContactInformation);

                foreach (var form in forms.AnsweredQuestions)
                {
                    var question = await _dbContext.Questions.Find(q => q.Id == form.QuestionId).FirstOrDefaultAsync();
                    if(question is null) continue;

                    var answeredQuestion = new ApplicantAnswer
                    {
                        Answer = form.Answer,
                        ProgramInfoId = programId,
                        QuestionId = question.Id,
                        QuestionType = question.QuestionType,
                    };

                    await _dbContext.ApplicantAnswers.InsertOneAsync(answeredQuestion);
                }

                return new BaseResponse<bool>(true, "Application submitted sucessfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, "Something went wrong.");
            }
        }

        public async Task<BaseResponse<GetApplicationFormQuestions>> GetApplicationFormQuestionsByProgramInfoId(ObjectId Id)
        {
            try
            {
                var program = await _dbContext.ProgramInfos.Find(q => q.Id == Id).FirstOrDefaultAsync();
                var questions = await _dbContext.Questions.Find(q => q.ProgramInfoId == Id).ToListAsync();

                if (questions is null || questions.Count == 0)
                {
                    return new BaseResponse<GetApplicationFormQuestions>(false, "Application Form Not Found");
                }


                var questionWithMultiChoicesTasks = questions.Select(async question =>
                {
                    var questionWithMultiChoice = new QuestionWithMultiChoice
                    {
                        Id = question.Id.ToString(),
                        Quesstion = question.Quesstion,
                        QuestionType = question.QuestionType,
                        ChoiceLimit = question.ChoiceLimit,
                        IsMultiChoice = question.QuestionType == QuestionType.MultiChoice,
                        ProgramInfoId = question.ProgramInfoId.ToString()
                    };

                    if (questionWithMultiChoice.IsMultiChoice)
                    {
                        var multiChoices = await _dbContext.MultiChoices.Find(mc => mc.QuestionId == question.Id).Project(mc => new MultiChoiceResponse
                        {
                            Choice = mc.Choice,
                            ProgramInfoId = question.ProgramInfoId.ToString(),
                            QuestionId = question.Id.ToString()
                        }).ToListAsync();

                        questionWithMultiChoice.MultiChoices = multiChoices;
                    }

                    return questionWithMultiChoice;
                });

                var questionWithMultiChoices = await Task.WhenAll(questionWithMultiChoicesTasks);

                return new BaseResponse<GetApplicationFormQuestions>
                {
                    Data = new GetApplicationFormQuestions
                    {
                        ProgramInfoId = Id.ToString(),
                        Questions = questionWithMultiChoices.ToList(),
                        ProgramInfo = new DTOs.ProgramInfoDto
                        {
                            Title = program.Title,
                            Description = program.Description,
                            IsDobRequired = program.IsDobRequired,
                            IsIdentificationNumberRequired = program.IsIdentificationNumberRequired,
                            IsGenderRequired = program.IsGenderRequired,
                            IsNationalityRequired = program.IsNationalityRequired,
                            IsPhoneNumberRequired = program.IsPhoneNumberRequired,
                            IsResidenceRequired = program.IsResidenceRequired,
                            IsEmailRequired = program.IsEmailRequired,
                            IsLastNameRequired = program.IsLastNameRequired,
                            IsFirstNameRequired = program.IsFirstNameRequired
                        }
                    },
                    Status = true,
                    Message = "Application frorm retrieved succesfully"

                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetApplicationFormQuestions>(false, "Something went wrong");
            }
        }
    }
}
