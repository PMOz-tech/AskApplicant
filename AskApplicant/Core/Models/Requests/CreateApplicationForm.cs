using AskApplicant.Core.DTOs;
using AskApplicant.Core.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace AskApplicant.Core.Models.Requests
{
    public class CreateApplicationForm
    {
        [Required]
        public ProgramInfoDto ProgramInfo { get; set; }
        public List<QuestionDto>? Question { get; set; }

    }

    public class CreateApplicationFormValidator : AbstractValidator<CreateApplicationForm>
    {
        public CreateApplicationFormValidator()
        {
            
            RuleForEach(x => x.Question)
           .Custom((questionDto, context) =>
           {
               if (questionDto.QuestionType == QuestionType.MultiChoice)
               {
                   if (questionDto.Choices == null)
                   {
                       context.AddFailure("Choices", "Choices cannot be null for MultiChoice question type");
                   }

                   if (questionDto.ChoiceLimit <= 0)
                   {
                       context.AddFailure("ChoiceLimit", "ChoiceLimit must be greater than zero for MultiChoice question type");
                   }
               }
           });
        }
    }
}
