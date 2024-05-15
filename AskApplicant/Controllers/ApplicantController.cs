using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.Models.Requests;
using AskApplicant.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace AskApplicant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _service;

        public ApplicantController(IApplicantService service)
        {
            _service = service;
        }

        /// <summary>
        /// gets all questions
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("{Id}/questions")]
        [ProducesResponseType(typeof(BaseResponse<GetApplicationFormQuestions>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromBody] string Id)
        {
            if (!ObjectId.TryParse(Id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format.");
            }

            var result = await _service.GetApplicationFormQuestionsByProgramInfoId(objectId);

            if (!result.Status) return BadRequest();

            return CreatedAtRoute(new { id = result }, result);

        }
        
        
        /// <summary>
        /// submoit filled form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost("review-questions")]
        [ProducesResponseType(typeof(BaseResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] FillApplicationFormRequest form)
        {
           
            var result = await _service.FillApplicationForm(form);

            if (!result.Status) return BadRequest();

            return CreatedAtRoute(new { id = result }, result);

        }
    }


}
