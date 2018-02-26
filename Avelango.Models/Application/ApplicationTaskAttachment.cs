using System;
using System.Runtime.Remoting.Messaging;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationTaskAttachment
    {
        public System.Guid PublicKey { get; set; }
        public string Url { get; set; }
        public string Extention { get; set; }
        public string FileTitle { get; set; }



        public static explicit operator ApplicationTaskAttachment(TaskAttachments task) {
            return new ApplicationTaskAttachment
            {
                PublicKey = task.PublicKey,
                Url = task.Url,
                Extention = task.Extention,
                FileTitle = task.FileTitle
            };
        }
    }
}
