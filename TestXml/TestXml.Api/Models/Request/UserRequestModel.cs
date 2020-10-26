﻿using TestXml.Abstract.Enums;

namespace TestXml.Api.Models.Request
{
    public class UserRequestModel
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