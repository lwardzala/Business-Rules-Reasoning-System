using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Reasoning.Core.Contracts;
using Reasoning.Host.Services;
using Reasoning.Host.Resources;

namespace Reasoning.Host.Controllers
{
    [ApiController]
    [Route("api/reasoning-task")]
    public class ReasoningTaskController : ControllerBase
    {
        private readonly IReasoningTaskService _reasoningTaskService;

        public ReasoningTaskController(IReasoningTaskService reasoningTaskService)
        {
            _reasoningTaskService = reasoningTaskService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReasoningTaskResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            ReasoningTaskResource result = await _reasoningTaskService.GetTaskAsync(id);

            if (result == null) return NotFound();
            
            return Ok(result);
        }

        [HttpGet("{id}/detail")]
        [ProducesResponseType(typeof(IReasoningTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(string id)
        {
            DetailedReasoningTaskResource result = await _reasoningTaskService.GetTaskDetailAsync(id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReasoningTaskResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateReasoningTaskResource reasoningTaskResource)
        {
            if (reasoningTaskResource.ReasoningMethod == ReasoningMethod.HypothesisTesting
                && reasoningTaskResource.Hypothesis == null)
                return BadRequest("Hypothesis parameter can't be null on `Hypothesis Testing` mode");

            try
            {
                return Ok(await _reasoningTaskService.CreateTaskAsync(reasoningTaskResource));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _reasoningTaskService.DeleteTaskAsync(id)) return Ok();

            return NotFound();
        }

        [HttpPut("{id}/variables")]
        [ProducesResponseType(typeof(MissingVariablesResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVariables(string id, [FromBody] VariablesResource variablesDto)
        {
            MissingVariablesResource result = await _reasoningTaskService.SetVariablesAsync(id, variablesDto);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}/resume")]
        [ProducesResponseType(typeof(ReasoningTaskResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Resume(string id)
        {
            ReasoningTaskResource result = await _reasoningTaskService.ResumeTaskAsync(id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(ReasoningTaskResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(string id)
        {
            ReasoningTaskResource result = await _reasoningTaskService.CancelTaskAsync(id);

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
