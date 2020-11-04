using TestXml.Abstract.Models;
using TestXml.Api.Models.Request;
using TestXml.Api.Models.Response;

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

        public static UserResponseModel AdaptModelToResponse(this UserInfo info)
        {
            var result = new UserResponseModel
            {
                UserId = info.UserId,
                UserName = info.UserName,
                UserStatus = info.UserStatus.ToString()
            };
            return result;
        }
    }
}