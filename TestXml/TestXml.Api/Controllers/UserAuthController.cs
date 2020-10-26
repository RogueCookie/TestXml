using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [Route("api/Auth/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserInfoService _infoService;

        public UserAuthController(IUserInfoService infoService)
        {
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model">Information about new user</param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserResponseModel>> CreateUser(UserRequestModel model)
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
        public Task<ActionResult<JsonObject>> RemoveUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change status of user
        /// </summary>
        /// <returns>Message weather or not user was deleted in Json format</returns>
        [HttpPost("SetStatus")]
        public async Task<ActionResult<JsonObject>> SetStatus([FromBody]int id, [FromBody] string newStatus) //TODo JsonObject from response
        {
            throw new NotImplementedException();
        }

    }
}
