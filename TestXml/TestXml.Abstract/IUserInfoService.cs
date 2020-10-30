using System.Collections.Generic;
using System.Threading.Tasks;
using TestXml.Abstract.Enums;
using TestXml.Abstract.Models;

namespace TestXml.Abstract
{
    public interface IUserInfoService
    {
        Task<UserInfo> CreateUser(int userId, string userName, UserStatus status);

        Task<List<UserInfo>> GetUsers();

        Task<UserInfo> Authenticate(string username, string password);
    }
}
