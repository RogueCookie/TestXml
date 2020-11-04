using System.Xml.Serialization;
using TestXml.Abstract.Enums;

namespace TestXml.Abstract.Models
{
    public class UserInfo
    {
        /// <summary>
        /// User Id
        /// </summary>
        [XmlAttribute("Id")]
        public int UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [XmlAttribute("Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Hold current user status
        /// </summary>
        [XmlElement("Status")]
        public UserStatus UserStatus { get; set; }

        /// <summary>
        /// Password for Authorize user
        /// </summary>
        [XmlIgnore]
        public string Password { get; set; }
    }
}