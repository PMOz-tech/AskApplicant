using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.DTOs;
using AskApplicant.Core.Entities;
using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using AskApplicant.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace AskApplicant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _service;

        public EmployerController(IEmployerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates an application form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost("create-form")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] CreateApplicationForm form)
        {
           var result =  await _service.CreateApplicationForm(form);

            if (!result.Status) return BadRequest();

            return CreatedAtRoute(new { id = result }, result);

        }
        
        
        /// <summary>
        /// Application Form Info with ProgramId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("{Id}/get-form")]
        [ProducesResponseType(typeof(BaseResponse<GetApplicationFormQuestions>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string Id)
        {
            if (!ObjectId.TryParse(Id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format.");
            }

            var result =  await _service.GetApplicationFormQuestionsByProgramInfoId(objectId);

            if (!result.Status)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Application Form Info with ProgramId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPut("update-form")]
        [ProducesResponseType(typeof(BaseResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put(List<EditApplicationForm> forms)
        {
           
            var result = await _service.EditApplicationForm(forms);

            if (!result.Status)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
