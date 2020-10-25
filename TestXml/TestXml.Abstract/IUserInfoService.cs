using System.Collections.Generic;
using System.Threading.Tasks;
using TestXml.Abstract.Models;

namespace TestXml.Abstract
{
    public interface IUserInfoService
    {
        Task<UserInfo> CreateUser(int userId, string userName, string status);
        List<UserInfo> GetUsers();
    }
}
