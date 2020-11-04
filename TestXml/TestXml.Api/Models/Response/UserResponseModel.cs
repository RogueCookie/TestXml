namespace TestXml.Api.Models.Response
{
    public class UserResponseModel
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
        public string UserStatus { get; set; } 
    }
}