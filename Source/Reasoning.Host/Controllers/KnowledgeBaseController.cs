using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Reasoning.Host.Services;
using Reasoning.Host.Resources;

namespace Reasoning.Host.Controllers
{
    [ApiController]
    [Route("api/knowledge-base")]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;

        public KnowledgeBaseController(IKnowledgeBaseService knowledgeBaseService)
        {
            _knowledgeBaseService = knowledgeBaseService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(KnowledgeBaseResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            KnowledgeBaseResource result = await _knowledgeBaseService.GetAsync(id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(KnowledgeBaseResource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] KnowledgeBaseResource knowledgeBaseResource)
        {
            KnowledgeBaseResource knowledgeBaseResponse = await _knowledgeBaseService.CreateAsync(knowledgeBaseResource.KnowledgeBase);

            if (knowledgeBaseResponse?.KnowledgeBase == null) return Conflict();

            return Created($"api/knowledge-base/{{{knowledgeBaseResponse.KnowledgeBase.Id}}}", knowledgeBaseResponse.KnowledgeBase);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(KnowledgeBaseResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody] KnowledgeBaseResource knowledgeBaseResource)
        {
            try
            {
                KnowledgeBaseResource result = await _knowledgeBaseService.UpdateAsync(id, knowledgeBaseResource.KnowledgeBase);
                if (result != null) return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _knowledgeBaseService.DeleteAsync(id)) return Ok();

            return NotFound();
        }
    }
}
