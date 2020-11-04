using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Abstract.Models.Options;
using TestXml.Api.Extension;
using TestXml.Api.Models.Response;

namespace TestXml.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class PublicController : Controller
    {
        private readonly IUserInfoService _infoService;
        private readonly IDistributedCache _cache;
        private readonly int _cachedHitLifeTime;

        /// <summary>
        /// key by which asking cash
        /// </summary>
        private const string JsonKey = "allUsers";

        public PublicController(IUserInfoService infoService, IDistributedCache cache, AppOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cachedHitLifeTime = options.CachedHitLifeTime;
        }

        /// <summary>
        /// Return information about users
        /// </summary>
        /// <returns>List of exist users</returns>
        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(List<UserResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<UserResponseModel>>> GetUsers()
        {
            // check is this record already in a cash
            var resultCash = await _cache.GetAsync(JsonKey);

            if (resultCash != null)
            {
                var cachedResultByte = await _cache.GetAsync(JsonKey);
                if (cachedResultByte != null)
                {
                    var cachedResultJson = Encoding.UTF8.GetString(cachedResultByte);
                    var cachedResult = JsonConvert.DeserializeObject<List<UserResponseModel>>(cachedResultJson);

                    return Ok(cachedResult);
                }
            }

            var result = await _infoService.GetUsers();
            var adaptResponseModel = result.Select(x => x.AdaptModelToResponse());

            var responseJson = JsonConvert.SerializeObject(adaptResponseModel);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            // assign how long we will be leave this cash 
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cachedHitLifeTime));

            await _cache.SetAsync(JsonKey, responseBytes, options);
            return Ok(adaptResponseModel);
        }

        /// <summary>
        /// Return information about user as html page
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> UserInfo([FromQuery] int id)
        {
           // check is this record already in a cash
            var resultCash = await _cache.GetAsync(JsonKey);
            var user = new UserResponseModel();

            if (resultCash != null)
            {
                var cachedResultByte = await _cache.GetAsync(JsonKey);
                if (cachedResultByte != null)
                {
                    var cachedResultJson = Encoding.UTF8.GetString(cachedResultByte);
                    var cachedResult = JsonConvert.DeserializeObject<List<UserResponseModel>>(cachedResultJson);

                    user = cachedResult.FirstOrDefault(x => x.UserId == id);

                    if (user == null) return null; //TODO

                    return Ok(user);
                }
            }

            var result = await _infoService.GetUsers();
            var adaptResponseModel = result.Select(x => x.AdaptModelToResponse());

            var responseJson = JsonConvert.SerializeObject(adaptResponseModel);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            // assign how long we will be leave this cash 
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cachedHitLifeTime));

            await _cache.SetAsync(JsonKey, responseBytes, options);

            user = adaptResponseModel.FirstOrDefault(x => x.UserId == id);

            return Ok(user);
        }
    }
}
