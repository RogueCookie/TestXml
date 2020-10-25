using System.ComponentModel.DataAnnotations;
using TestXml.Abstract.Enums;

namespace TestXml.Api.Models.Request
{
    public class UserRequestModel
    {
        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Hold current user status
        /// </summary>
        //[Required]
        //[EnumDataType(typeof(UserStatus))]
        //[JsonConverter(typeof(StringEnumConverter<,,>))]
        public string UserStatus { get; set; }
    }
}