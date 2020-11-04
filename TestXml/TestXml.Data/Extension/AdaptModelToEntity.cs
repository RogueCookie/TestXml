using TestXml.Abstract.Models;
using TestXml.Data.Entities;

namespace TestXml.Data.Extension
{
    public static class AdaptModelToEntity
    {
        /// <summary>
        /// Adapt user model to entity 
        /// </summary>
        /// <param name="info">Model</param>
        /// <returns>Adapt entity</returns>
        public static UserInfoEntity AdaptUserToEntity (this UserInfo info)
        {
            var result = new UserInfoEntity()
            {
               UserId = info.UserId,
               UserName = info.UserName,
               UserStatus = info.UserStatus
            };
            return result;
        }

        /// <summary>
        /// Adapt entity to user model
        /// </summary>
        /// <param name="entity"><Entity</param>
        /// <returns>Adapt model</returns>
        public static UserInfo AdaptEntityToUserInfoModel(this UserInfoEntity entity)
        {
            var result = new UserInfo()
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                UserStatus = entity.UserStatus
            };
            return result;
        }
    }
}