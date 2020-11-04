using System.Collections.Generic;
using System.Threading.Tasks;
using TestXml.Abstract.Models;

namespace TestXml.Abstract
{
    /// <summary>
    /// Main manipulation with User 
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="model">New user model</param>
        /// <returns>User model which was added</returns>
        Task<UserInfo> CreateUser(UserInfo model);

        /// <summary>
        /// Get all information about user
        /// </summary>
        /// <returns>List of exist users</returns>
        Task<List<UserInfo>> GetUsers();

        /// <summary>
        /// Used for base authentication
        /// </summary>
        Task<UserInfo> Authenticate(string username, string password);

        /// <summary>
        /// Remove user by Id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Model with updated status</returns>
        Task<UserInfo> RemoveUser(int userId);

        /// <summary>
        /// Change user status
        /// </summary>
        /// <param name="userId">Id of user for updates</param>
        /// <param name="status">New status which need to assign for user</param>
        /// <returns>Updated user model</returns>
        Task<UserInfo> SetStatus(int userId, string status);
    }
}
