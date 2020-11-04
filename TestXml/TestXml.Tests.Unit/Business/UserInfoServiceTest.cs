using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models;
using TestXml.Abstract.Models.Exceptions;
using TestXml.Business;
using TestXml.Data;
using TestXml.Data.Entities;
using Xunit;

namespace TestXml.Tests.Unit.Business
{
    public class UserInfoServiceTest
    {
        private readonly UserInfoService _userService;
        private readonly TestXmlDbContext _context;

        public UserInfoServiceTest()
        {
            // for test create db in memory
            _context = new TestXmlDbContext(new DbContextOptionsBuilder<TestXmlDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _userService = new UserInfoService(_context);
        }

        [Fact]
        public async Task GetUserInfo_WhenGot_ListOfAllUsersExpected()
        {
            // Arrange
            var initUsers = InitListOfUser();
            await _context.AddRangeAsync();
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.True(result.Count == 3);
            foreach (var user in result)
            {
                Assert.True(user.UserStatus != UserStatus.Deleted);
            }
        }

        [Fact]
        public async Task CreateUser_WhenDone_AddedUserExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            initUser.UserId = 6;
            var adaptUserModel = AdaptToModel(initUser);

            // Act
            var result = await _userService.CreateUser(adaptUserModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(initUser.UserName, result.UserName);
            Assert.Equal(initUser.UserStatus, result.UserStatus);
        }

        [Fact]
        public async Task CreateUser_WhenIdAlreadyExist_CustomExceptionExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            var id = await _context.SaveChangesAsync();
            var model = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            var adaptModel = AdaptToModel(model);

            // Act
            async Task ActAsync() => await _userService.CreateUser(adaptModel);

            // Assert
            await Assert.ThrowsAsync<XmlExceptionError1>(ActAsync);
        }

        [Fact]
        public async Task RemoveUser_WhenIdNotExist_CustomExceptionExpected()
        {
            // Arrange
            var id = 5;

            // Act
            async Task ActAsync() => await _userService.RemoveUser(id);

            // Assert
            await Assert.ThrowsAsync<XmlExceptionError2>(ActAsync);
        }

        [Fact]
        public async Task RemoveUser_WhenDone_TrueExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.RemoveUser(initUser.UserId);
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == initUser.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(initUser.UserName, userInDb.UserName);
            Assert.True(userInDb.UserStatus == UserStatus.Deleted);
        }

        [Fact]
        public async Task SetStatus_WhenNewStatusAssign_UpdatedUserExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.SetStatus(initUser.UserId, UserStatus.Blocked.ToString());
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == initUser.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(initUser.UserName, userInDb.UserName);
            Assert.True(userInDb.UserStatus == UserStatus.Blocked);
        }

        [Fact]
        public async Task SetStatus_WhenNewStatusNotFromEnum_CustomExceptionExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            await _context.SaveChangesAsync();

            // Act
            async Task ActAsync() => await _userService.SetStatus(initUser.UserId, "Unknown"); ;

            // Assert
            await Assert.ThrowsAsync<XmlExceptionError1>(ActAsync);
        }

        [Fact]
        public async Task SetStatus_WhenUserNotExist_CustomExceptionExpectedExpected()
        {
            // Arrange

            // Act
            async Task ActAsync() => await _userService.SetStatus(10000, "Unknown"); ;

            // Assert
            await Assert.ThrowsAsync<XmlExceptionError2>(ActAsync);
        }

        private UserInfoEntity InitUserEntity()
        {
            return new UserInfoEntity()
            {
                UserName = "test",
                UserStatus = UserStatus.Active
            };
        }

        private static UserInfo AdaptToModel(UserInfoEntity entity)
        {
            return new UserInfo()
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                UserStatus = entity.UserStatus
            };
        }

        private static List<UserInfoEntity> InitListOfUser()
        {
            var users = new List<UserInfoEntity>()
            {
                new UserInfoEntity() {UserId = 1, UserName = "tom@gmail.com", UserStatus = UserStatus.Active},
                new UserInfoEntity() {UserId = 2, UserName = "alice@yahoo.com", UserStatus = UserStatus.Blocked},
                new UserInfoEntity() {UserId = 3, UserName = "sam@online.com", UserStatus = UserStatus.Deleted},
                new UserInfoEntity() {UserId = 4, UserName = "val@online.com", UserStatus = UserStatus.New}
            };
            return users;
        }
    }
}