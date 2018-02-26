using System;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationNews
    {
        public Guid PublicKey { get; set; }
        public string Html { get; set; }
        public DateTime Created { get; set; }


        public static explicit operator ApplicationNews(CommonNews news) {
            return new ApplicationNews {
                PublicKey = news.PublicKey,
                Html = news.Html,
                Created = news.Created,
            };
        }
    }
}
