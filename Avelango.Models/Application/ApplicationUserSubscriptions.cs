namespace Avelango.Models.Application
{
    public class ApplicationUserSubscriptions
    {
        public string SubscribedGroups { get; set; }
        public bool EmailDelivery { get; set; }
        public bool SmsDelivery { get; set; }
        public bool PushUpDelivery { get; set; }

        public ApplicationUserSubscriptions(string subscribedGroups, bool? emailDelivery, bool? smsDelivery, bool? pushUpDelivery) {
            SubscribedGroups = subscribedGroups;
            EmailDelivery = emailDelivery ?? false;
            SmsDelivery = smsDelivery ?? false;
            PushUpDelivery = pushUpDelivery ?? false;
        }
    }
}
