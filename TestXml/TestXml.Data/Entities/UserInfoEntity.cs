using System;
using System.ComponentModel.DataAnnotations;
using TestXml.Abstract.Enums;

namespace TestXml.Data.Entities
{
    public class UserInfoEntity
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Hold current user status
        /// </summary>
        public UserStatus UserStatus { get; set; }
    }
}