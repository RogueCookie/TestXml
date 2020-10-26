using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Abstract.Models;
using TestXml.Abstract.Models.Options;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [ApiController]
    [Route("api/Public/[controller]")]
    public class UserController : Controller
    {
        //private readonly ILogger<UserController> _logger;
        private readonly IUserInfoService _infoService;
        private readonly IDistributedCache _cache;
        private readonly int _cachedHitLifeTime;

        public UserController(/*ILogger<UserController> logger,*/ IUserInfoService infoService, IDistributedCache cache, AppOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cachedHitLifeTime = options.CachedHitLifeTime;
        }

        /// <summary>
        /// Return information about user as html page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("CreateUser")] //TODO user info need to replace
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]// TODO list
        public async Task<ActionResult<UserResponseModel>> CreateUser(UserRequestModel model) //TODO
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // key by which asking cash
            var jsonKey = JsonConvert.SerializeObject(model); 
            
            // check is this record already in a cash
            var resultCash = await _cache.GetAsync(jsonKey); 

            if (resultCash != null)
            {
                var cachedResultByte = await _cache.GetAsync(jsonKey);
                if (cachedResultByte != null)
                {
                    var cachedResultJson = Encoding.UTF8.GetString(cachedResultByte);
                    var cachedResult = JsonConvert.DeserializeObject<UserInfo>(cachedResultJson);
                    return Ok(cachedResult);
                }
            }

            var result = await _infoService.CreateUser(model.UserId, model.UserName, model.UserStatus);
            if (result == null) NotFound();

            var responseJson = JsonConvert.SerializeObject(result);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            // assign how long we will be leave this cash 
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cachedHitLifeTime)); 

            await _cache.SetAsync(jsonKey, responseBytes, options);

            return Ok(result);
        }

        [HttpGet("test")]
        public Task<List<UserInfo>> CreateUser()
        {
            var r =_infoService.GetUsers();
            return Task.FromResult(r);
        }


    }
}
