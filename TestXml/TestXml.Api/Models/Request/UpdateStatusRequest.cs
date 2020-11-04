using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestXml.Api.Models.Request
{
    public class UpdateStatusRequest
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Status which need to assign to particular user
        /// </summary>
        public string NewStatus { get; set; }
    }
}
