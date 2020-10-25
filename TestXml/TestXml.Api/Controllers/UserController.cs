using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestXml.Abstract;
using TestXml.Abstract.Models;
using TestXml.Api.Models.Request;
using TestXml.Data.Entities;

namespace TestXml.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserInfoService _infoService;

        public UserController(ILogger<UserController> logger, IUserInfoService infoService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
        }

        [HttpGet("AddUser")]
        public async Task<ActionResult<UserInfo>> CreateUser(UserRequestModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var result = await _infoService.CreateUser(model.UserId, model.UserName, model.UserStatus);
            
            return result;
        }

        [HttpGet("test")]
        public async Task<List<UserInfo>> CreateUser()
        {
            var r =_infoService.GetUsers();
            return r;
        }


    }
}
