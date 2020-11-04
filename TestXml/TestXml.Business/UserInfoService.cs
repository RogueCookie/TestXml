using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestXml.Abstract;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models;
using TestXml.Data;
using TestXml.Data.Entities;
using TestXml.Data.Extension;

namespace TestXml.Business
{
    public class UserInfoService : IUserInfoService
    {
        private readonly TestXmlDbContext _dbContext;

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<UserInfo> _users = new List<UserInfo> {new UserInfo() { UserId = 1, UserName = "Test", Password = "test" }};

        public UserInfoService(TestXmlDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<List<UserInfo>> GetUsers()
        {
            var foo =  await _dbContext.Users.Where(x => x.UserStatus != UserStatus.Deleted).ToListAsync();
            var result = foo.Select(x => AdaptUser(x)).ToList();
            //var result2 = await Task.Run(() => result.WithoutPasswords().ToList());
            return result;
        }


        /// <inheritdoc />
        public async Task<UserInfo> CreateUser(UserInfo model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var existUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            if (existUser != null) return null; //TODO error model

            var modelForAdd = model.AdaptUserToEntity();
            await _dbContext.Users.AddAsync(modelForAdd);
            await _dbContext.SaveChangesAsync();

            var result =  await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
           
            return result?.AdaptEntityToUserInfoModel();
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

        /// <inheritdoc />
        public async Task<UserInfo> RemoveUser(int userId)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existUser == null) return null;

            existUser.UserStatus = UserStatus.Deleted;

            _dbContext.Users.Update(existUser);
            await _dbContext.SaveChangesAsync();

            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return result?.AdaptEntityToUserInfoModel();
        }

        /// <inheritdoc />
        public async Task<UserInfo> SetStatus(int userId, string status)
        {
            if (status == null) throw new ArgumentNullException(nameof(status));

            var existUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existUser == null) return null; //TODO
            var currentStatus = existUser.UserStatus.ToString();

            var newStatusIsEnum = Enum.IsDefined(typeof(UserStatus), status);//TODO check
            if (!newStatusIsEnum) return null;

            if (currentStatus == status) return existUser?.AdaptEntityToUserInfoModel();

            var newStatus = (UserStatus)Enum.Parse(typeof(UserStatus), status);
            existUser.UserStatus = newStatus;

            _dbContext.Users.Update(existUser);
            await _dbContext.SaveChangesAsync();

            var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return updatedUser?.AdaptEntityToUserInfoModel();
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