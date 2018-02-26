using System;

namespace Avelango.Models.Application
{
    public class ApplicationComments
    {
        public Guid PublicKey { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
    }
}
