using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.DTOs;
using AskApplicant.Core.Entities;
using AskApplicant.Core.Enums;
using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using AskApplicant.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AskApplicant.Core.Application.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly AskApplicantDbContext _dbContext;
        public EmployerService(AskApplicantDbContext dbContext)
        {
            _dbContext = dbContext;   
        }

        public async Task<BaseResponse<string>> CreateApplicationForm(CreateApplicationForm applicationForm)
        {
            try
            {
                ProgramInfo newProgramInfo = new ProgramInfo
                {
                    Description = applicationForm.ProgramInfo.Description,
                    Title = applicationForm.ProgramInfo.Title,
                };

               await _dbContext.ProgramInfos.InsertOneAsync(newProgramInfo);


                Question newQuestion;
                MultiChoice newMultiChoice = new MultiChoice();
                if(applicationForm.Question is not null)
                {
                    foreach(var question in applicationForm.Question)
                    {

                         newQuestion = new Question
                        {
                            ChoiceLimit = question.ChoiceLimit.GetValueOrDefault(),
                            ProgramInfoId = newProgramInfo.Id,
                            Quesstion = question.Quesstion,
                            QuestionType = question.QuestionType,
                            IsMultiChoice = question.QuestionType == QuestionType.MultiChoice
                            
                        };

                        await _dbContext.Questions.InsertOneAsync(newQuestion);

                        if (question.QuestionType == QuestionType.MultiChoice && question.Choices != null && question.ChoiceLimit != null)
                        {
                            foreach (var choiceDto in question.Choices.Where(c => !string.IsNullOrEmpty(c.Choice)))
                            {

                                newMultiChoice.Choice = choiceDto.Choice;
                                newMultiChoice.QuestionId = newQuestion.Id;
                                newMultiChoice.ProgramInfoId = newProgramInfo.Id;
                                await _dbContext.MultiChoices.InsertOneAsync(newMultiChoice);
                            }

                          
                        }
                    }
                }

                return new BaseResponse<string>
                {
                    Data = newProgramInfo.Id.ToString(),
                    Status = true
                };
               
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>(false, "Something went wrong");
            }
        }

        public async Task<BaseResponse<bool>> EditApplicationForm(List<EditApplicationForm> forms)
        {
            try
            {
                foreach(var form in forms)
                {
                    ObjectId questionId;
                    if (!ObjectId.TryParse(form.QuestionId, out questionId))
                    {
                        // Handle invalid ObjectId
                        continue;
                    }

                    var question = await _dbContext.Questions.Find(q => q.Id == questionId).FirstOrDefaultAsync();
                    if (question is null) continue;

                    if(question.QuestionType != form.QuestionType && form.QuestionType == QuestionType.MultiChoice && form.MultiChoices is not null && form.ChoiceLimit is not null)
                    {
                        var multiChoices = form.MultiChoices.Select(choice => new MultiChoice
                        {
                            Choice = choice.Choice,
                            ProgramInfoId = question.ProgramInfoId,
                            QuestionId = questionId,
                        }).ToList();

                        await _dbContext.MultiChoices.InsertManyAsync(multiChoices);

                        question.ChoiceLimit = form.ChoiceLimit.GetValueOrDefault();

                    }

                    question.Quesstion = string.IsNullOrWhiteSpace(form.Question) || string.Equals(question.Quesstion, form.Question) ? question.Quesstion : form.Question;
                    question.QuestionType = form.QuestionType == question.QuestionType ? question.QuestionType : form.QuestionType;


                    await _dbContext.Questions.ReplaceOneAsync(q => q.Id == questionId, question);

                }

                return new BaseResponse<bool>(true, "Application form updated");
            }
            catch(Exception ex)
            {
                return new BaseResponse<bool>(false, "Something went wrong");
            }
        }

        public async Task<BaseResponse<GetApplicationFormQuestions>> GetApplicationFormQuestionsByProgramInfoId(ObjectId Id)
        {
            try
            {
                var questions = await _dbContext.Questions.Find(q => q.ProgramInfoId == Id).ToListAsync();

                if(questions is null || questions.Count == 0)
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
                        ProgramInfo = null
                    },
                    Status = true,
                    Message = "Application frorm retrieved succesfully"
                 
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<GetApplicationFormQuestions>(false, "Something went wrong");
            }
        }

    
    }
}
