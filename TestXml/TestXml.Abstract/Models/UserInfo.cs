using System;
using TestXml.Abstract.Enums;

namespace TestXml.Abstract.Models
{
    /// <summary>
    /// User information
    /// </summary>
    public class UserInfo
    {
        public UserInfo(int userId, string userName, UserStatus userStatus)
        {
            UserId = userId;

            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            UserName = userName;

            UserStatus = userStatus;
        }

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