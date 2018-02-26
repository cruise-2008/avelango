using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationNotifycation
    {
        public bool Reviewed { get; set; }
        public System.DateTime Created { get; set; }
        public string NotificationType { get; set; }
        public System.Guid PublicKey { get; set; }


        public object Task { get; set; }
        public object FromUser { get; set; }


        public static explicit operator ApplicationNotifycation(Notifications noti) {
            return new ApplicationNotifycation {
                Reviewed = noti.Reviewed,
                Created = noti.Created,
                NotificationType = noti.NotificationType,
                PublicKey = noti.PublicKey
            };
        }
    }
}
