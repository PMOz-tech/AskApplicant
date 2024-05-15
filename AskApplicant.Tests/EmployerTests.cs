using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.Application.Services;
using AskApplicant.Core.DTOs;
using AskApplicant.Core.Models.Requests;
using AskApplicant.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskApplicant.Tests
{
    public class EmployerTests
    {
        private readonly EmployerService _employerService;
        private readonly Mock<AskApplicantDbContext> _context;

        public EmployerTests()
        {
            _context = new Mock<AskApplicantDbContext>();

            _employerService = new EmployerService(_context.Object);
        }

        [Fact]
        public async Task CreateApplicationForm_When_Form_Is_Valid()
        {
            //arrange
            var program = new ProgramInfoDto { Title = "Internship", Description = "Valid", IsIdentificationNumberRequired = true };
            var questions = new List<QuestionDto>
            {
                new QuestionDto
                {
                    Quesstion = "What's your notice period",
                    QuestionType = Core.Enums.QuestionType.Numeric
                }
            };
            var form = new CreateApplicationForm { ProgramInfo = program, Question = questions };

            //act
            var result = await _employerService.CreateApplicationForm(form);

            //assert
            Assert.NotNull(result.Data);


        }
    }
}
