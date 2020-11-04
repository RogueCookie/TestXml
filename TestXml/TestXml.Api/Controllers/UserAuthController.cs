using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Abstract.Models;
using TestXml.Api.Extension;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
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
        //[Produces("application/xml")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserRequestModel>> CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var adaptModel = model.AdaptRequestToModel();

            var result = await _infoService.CreateUser(adaptModel); 
            if (result == null) NotFound();//TODO

            return Ok(result);
        }

        /// <summary>
        /// Remove particular user
        /// </summary>
        /// <returns>Message weather or not user was deleted in Json format</returns>
        [HttpPost("RemoveUser")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> RemoveUser(int userId)
        {
            var markUser = await _infoService.RemoveUser(userId);
            if(markUser == null) return null;
           
            return Ok(markUser);
        }

        /// <summary>
        /// Change status of user
        /// </summary>
        /// <returns>Message weather or not user was deleted in Json format</returns>
        [HttpPost("SetStatus")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> SetStatus(int id, string newStatus) //TODo JsonObject from response
        {
            throw new NotImplementedException();
        }
    }
}
