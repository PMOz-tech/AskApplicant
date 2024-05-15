using AskApplicant.Core.DTOs;
using AskApplicant.Core.Entities;
using AskApplicant.Core.Enums;
using FluentValidation;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace AskApplicant.Core.Models.Requests
{
    public class EditApplicationForm
    {
    
        public string QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? Question { get; set; }
        public int? ChoiceLimit { get; set; } = 0;
        public List<MultiChoiceDto>? MultiChoices { get; set; }

    }

    public class EditApplicationFormValidator : AbstractValidator<EditApplicationForm>
    {
        public EditApplicationFormValidator()
        {

            RuleFor(x => x)
            .Custom((form, context) =>
            {
                if (form.QuestionType == QuestionType.MultiChoice)
                {
                    if (form.MultiChoices == null)
                    {
                        context.AddFailure(nameof(form.MultiChoices), "MultiChoices cannot be null for MultiChoice question type");
                    }
                    else if (form.ChoiceLimit <= 0)
                    {
                        context.AddFailure(nameof(form.ChoiceLimit), "ChoiceLimit must be greater than zero for MultiChoice question type");
                    }
                }
            });
        }
    }
}
