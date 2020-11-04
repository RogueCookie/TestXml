using System;
using System.Xml.Serialization;
using TestXml.Abstract.Models;

namespace TestXml.Api.Models.Response
{
    [XmlRoot("Response")]
    public class Response
    {
        [XmlElement("user")]
        public UserInfo User { get; set; }

        [XmlAttribute("Success")]
        public bool IsSuccess { get; set; }

        [XmlAttribute("ErrorId")]
        public int ErrorId { get; set; }

        [XmlElement("ErrorMsg")]
        public string ErrorMsg { get; set; }

        public Response(bool result, Exception ex, int errorId)
        {
            IsSuccess = result;
            ErrorMsg = ex?.Message;
            ErrorId = errorId;
        }
    }
}