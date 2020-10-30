using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestXml.Abstract;
using TestXml.Abstract.Models;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [Route("api/Auth/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserInfoService _infoService;

        public UserAuthController(IUserInfoService infoService)
        {
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = await _infoService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model">Information about new user</param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        [Produces("application/xml")]
        [Consumes("application/xml")]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody]UserRequestModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = await _infoService.CreateUser(model.UserId, model.UserName, model.UserStatus); //TODO XML
            if (result == null) NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Remove particular user
        /// </summary>
        /// <returns>Message weather or not user was deleted in Json format</returns>
        [HttpPost("RemoveUser")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public Task<ActionResult<UserResponseModel>> RemoveUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change status of user
        /// </summary>
        /// <returns>Message weather or not user was deleted in Json format</returns>
        [HttpPost("SetStatus")]
        public async Task<ActionResult<UserResponseModel>> SetStatus(int id,  string newStatus) //TODo JsonObject from response
        {
            throw new NotImplementedException();
        }
    }
}
