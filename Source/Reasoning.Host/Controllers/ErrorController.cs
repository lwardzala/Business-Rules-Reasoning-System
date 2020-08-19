using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace Reasoning.Host.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error-local")]
        public IActionResult ErrorLocal()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(detail: context.Error.StackTrace, title: context.Error.Message);
        }

        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
