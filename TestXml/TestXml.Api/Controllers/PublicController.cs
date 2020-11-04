using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestXml.Abstract;
using TestXml.Abstract.Models;
using TestXml.Abstract.Models.Options;
using TestXml.Api.Extension;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;
using TestXml.Data.Extension;

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

        public PublicController(IUserInfoService infoService, IDistributedCache cache, AppOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cachedHitLifeTime = options.CachedHitLifeTime;
        }

        /// <summary>
        /// Return information about user as html page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")] 
        [ProducesResponseType(typeof(List<UserResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<UserResponseModel>>> GetUsers() 
        {
            // key by which asking cash
            var jsonKey = "allUsers"; 
            
            // check is this record already in a cash
            var resultCash = await _cache.GetAsync(jsonKey); 

            if (resultCash != null)
            {
                var cachedResultByte = await _cache.GetAsync(jsonKey);
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

            await _cache.SetAsync(jsonKey, responseBytes, options);
            return Ok(adaptResponseModel);
        }
    }
}
