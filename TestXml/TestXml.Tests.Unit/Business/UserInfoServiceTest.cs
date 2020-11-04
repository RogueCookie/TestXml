using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models;
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
        }

        [Fact]
        public async Task CreateUSer_WhenDone_AddedUserExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            initUser.UserId = 6;
            var adaptUserModel = AdaptToModel(initUser);

            // Act
            var result = await _userService.CreateUser(adaptUserModel);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateUser_WhenIdAlreadyExist_NullExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            var id = await _context.SaveChangesAsync();
            var model = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            var adaptModel = AdaptToModel(model);

            // Act
            var result = await _userService.CreateUser(adaptModel);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveUser_WhenIdNotExist_NullExpected()
        {
            // Arrange
            var id = 5;

            // Act
            var result = await _userService.RemoveUser(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveUser_WhenDone_TrueExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            var id = await _context.SaveChangesAsync();

            // Act
            var result = await _userService.RemoveUser(id);
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetStatus_WhenNewStatusAssign_UpdatedUserExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            var id = await _context.SaveChangesAsync();

            // Act
            var result = await _userService.SetStatus(id, UserStatus.Blocked.ToString());
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Assert
            Assert.NotNull(result);
            Assert.True(userInDb.UserStatus == UserStatus.Blocked);
        }

        [Fact]
        public async Task SetStatus_WhenNewStatusNotFromEnum_UserExpected()
        {
            // Arrange
            var initUser = InitUserEntity();
            await _context.Users.AddAsync(initUser);
            var id = await _context.SaveChangesAsync();
            var userBefore = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Act
            var result = await _userService.SetStatus(id, "Unknown");
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Assert
            Assert.Null(result);
            Assert.NotNull(userBefore);
            Assert.Equal(userBefore.UserStatus, userInDb.UserStatus);
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