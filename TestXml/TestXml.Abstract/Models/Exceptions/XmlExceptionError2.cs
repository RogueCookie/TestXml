using System;

namespace TestXml.Abstract.Models.Exceptions
{
    /// <summary>
    /// General model for passing additional message with errorId 2
    /// </summary>
    public class XmlExceptionError2 : Exception
    {
        public XmlExceptionError2(string message) : base(message)
        {

        }
    }
}