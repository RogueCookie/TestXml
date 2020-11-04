using TestXml.Abstract.Models;
using TestXml.Api.Models.Request;

namespace TestXml.Api.Extension
{
    public static class AdaptModels
    {
        public static UserInfo AdaptRequestToModel(this UserRequestModel info)
        {
            var result = new UserInfo()
            {
                UserId = info.UserId,
                UserName = info.UserName,
                UserStatus = info.UserStatus
            };
            return result;
        }
    }
}