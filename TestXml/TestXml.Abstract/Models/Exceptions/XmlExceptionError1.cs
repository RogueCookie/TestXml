using System;

namespace TestXml.Abstract.Models.Exceptions
{
    /// <summary>
    /// General model for passing additional message with errorId 1
    /// </summary>
    public class XmlExceptionError1 : Exception
    {
        public XmlExceptionError1(string message) : base(message)
        {
            
        }
    }
}