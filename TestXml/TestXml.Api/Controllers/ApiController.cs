using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TestXml.Abstract.Models.Exceptions;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiController : ControllerBase
    {
        [Route("/error")]
        public Response Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var code = exception switch
            {
                XmlExceptionError1 _ => 1,
                XmlExceptionError2 _ => 2,
                _ => 500
            };

            return new Response( false, exception, code);
        }

        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}
