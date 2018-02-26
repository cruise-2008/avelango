namespace Avelango.Models.Application
{
    public class ApplicationMessage
    {
        public bool IsNew { get; set; }
        public System.DateTime Created { get; set; }
        public string Message { get; set; }
        public string Attachments { get; set; }
        public bool MyOwn { get; set; }

    }
}
