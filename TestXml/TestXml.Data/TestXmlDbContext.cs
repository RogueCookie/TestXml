using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models.Options;
using TestXml.Data.Entities;

namespace TestXml.Data
{
    public class TestXmlDbContext : DbContext
    {
        public DbSet<UserInfoEntity> Users { get; set; }

        public TestXmlDbContext(DbContextOptions<TestXmlDbContext> options)
            : base(options)
        {
            LoadDefaultUsers();
        }

        public List<UserInfoEntity> GetUsers() => Users.Local.ToList();

        private void LoadDefaultUsers()
        {
            Users.Add(new UserInfoEntity() {UserId = 1, UserName = "tom@gmail.com", UserStatus = UserStatus.Active});
            Users.Add(new UserInfoEntity() {UserId = 2, UserName = "alice@yahoo.com", UserStatus = UserStatus.Blocked});
            Users.Add(new UserInfoEntity() {UserId = 3, UserName = "sam@online.com", UserStatus = UserStatus.Deleted});
            Users.Add(new UserInfoEntity() {UserId = 4, UserName = "val@online.com", UserStatus = UserStatus.New});
        }


        //private readonly AppOptions _options;
        //private readonly ILoggerFactory _loggerFactory;

        //public TestXmlDbContext(AppOptions options, ILoggerFactory loggerFactory)
        //{
        //    _options = options ?? throw new ArgumentNullException(nameof(options));
        //    _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        //    if (string.IsNullOrEmpty(options.DataBaseConnectionString)) throw new ArgumentException(nameof(options.DataBaseConnectionString));
        //}



    }
}