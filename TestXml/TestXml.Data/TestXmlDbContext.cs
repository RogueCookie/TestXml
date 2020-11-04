using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models.Options;
using TestXml.Data.Entities;

namespace TestXml.Data
{
    public class TestXmlDbContext :  DbContext
    {
        public TestXmlDbContext(DbContextOptions<TestXmlDbContext> option) : base(option)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserInfoEntity> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfoEntity>().ToTable("Users").HasKey(x => x.UserId);
            var users = new List<UserInfoEntity>()
            {
                new UserInfoEntity() {UserId = 1, UserName = "tom@gmail.com", UserStatus = UserStatus.Active},
                new UserInfoEntity() {UserId = 2, UserName = "alice@yahoo.com", UserStatus = UserStatus.Blocked},
                new UserInfoEntity() {UserId = 3, UserName = "sam@online.com", UserStatus = UserStatus.Deleted},
                new UserInfoEntity() {UserId = 4, UserName = "val@online.com", UserStatus = UserStatus.New}
            };
            modelBuilder.Entity<UserInfoEntity>().HasData(users);
            base.OnModelCreating(modelBuilder);
        }

        //public List<UserInfoEntity> GetUsers() => Users.Local.ToList();
    }
}