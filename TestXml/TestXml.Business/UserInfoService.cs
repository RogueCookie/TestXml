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
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<UserInfo> _users = new List<UserInfo> {new UserInfo() { UserId = 1, UserName = "Test", Password = "test" }};

        public UserInfoService(AppOptions options, TestXmlDbContext dbContext)
        {
            _dbContext = dbContext;
             _options = options ?? throw new ArgumentNullException(nameof(options));
        }


        //public void Initialize()
        //{
        //    if (!_dbContext.Users.Any())
        //    {
        //        _dbContext.Users.AddRange(
        //            new UserInfoEntity() { UserId = 1, UserName = "tom@gmail.com", UserStatus = UserStatus.Active },
        //            new UserInfoEntity() { UserId = 2, UserName = "alice@yahoo.com", UserStatus = UserStatus.Blocked },
        //            new UserInfoEntity() { UserId = 3, UserName = "sam@online.com", UserStatus = UserStatus.Deleted },
        //            new UserInfoEntity() { UserId = 3, UserName = "val@online.com", UserStatus = UserStatus.New }
        //        );
        //        _dbContext.SaveChanges();
        //    }
        //}

        /// <inheritdoc />
        public async Task<List<UserInfo>> GetUsers()
        {
            var foo =  _dbContext.GetUsers();
            var result = foo.Select(x => AdaptUser(x)).ToList();
            var result2 = await Task.Run(() => result.WithoutPasswords().ToList());
            return result;
        }


        /// <inheritdoc />
        public Task<UserInfo> CreateUser(int userId, string userName, UserStatus status)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<UserInfo> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            var user = await Task.Run(() => _users.SingleOrDefault(x => x.UserName == username && x.Password == password));

            if (user == null) return null; //TODO handle error

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }


        private UserInfo AdaptUser(UserInfoEntity userInfoEntity)
        {
            return new UserInfo()
            {
                UserId = userInfoEntity.UserId,
                UserName = userInfoEntity.UserName,
                UserStatus = userInfoEntity.UserStatus
            };
        }
    }
}