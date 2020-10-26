using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models;
using TestXml.Abstract.Models.Options;
using TestXml.Data;
using TestXml.Data.Entities;

namespace TestXml.Business
{
    public class UserInfoService : IUserInfoService
    {
        private readonly TestXmlDbContext _dbContext;

        private readonly IMemoryCache _memoryCache;
        private readonly AppOptions _options;

        public UserInfoService(AppOptions options, TestXmlDbContext dbContext/*, IMemoryCache memoryCache*/)
        {
            _dbContext = dbContext;
            //_memoryCache = memoryCache;
             _options = options ?? throw new ArgumentNullException(nameof(options));
        }


        public void Initialize()
        {
            if (!_dbContext.Users.Any())
            {
                _dbContext.Users.AddRange(
                    new UserInfoEntity() { UserId = 1, UserName = "tom@gmail.com", UserStatus = UserStatus.Active },
                    new UserInfoEntity() { UserId = 2, UserName = "alice@yahoo.com", UserStatus = UserStatus.Blocked },
                    new UserInfoEntity() { UserId = 3, UserName = "sam@online.com", UserStatus = UserStatus.Deleted },
                    new UserInfoEntity() { UserId = 3, UserName = "val@online.com", UserStatus = UserStatus.New }
                );
                _dbContext.SaveChanges();
            }
        }

        public List<UserInfo> GetUsers()
        {
            var foo = _dbContext.GetUsers();
            var result = foo.Select(x => AdaptUser(x)).ToList();
            return result;
        }

        private UserInfo AdaptUser(UserInfoEntity userInfoEntity)
        {
            return new UserInfo(userInfoEntity.UserId, userInfoEntity.UserName, userInfoEntity.UserStatus)
            {
                UserId = userInfoEntity.UserId,
                UserName = userInfoEntity.UserName,
                UserStatus = userInfoEntity.UserStatus
            };
        }

        public Task<UserInfo> CreateUser(int userId, string userName, UserStatus status)
        {
            throw new System.NotImplementedException();
        }
    }
}