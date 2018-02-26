using System;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationWorker
    {
        public string Name { get; set; }
        public string UserLogoPath { get; set; }
        public double? Rating { get; set; }
        public Guid PublicKey { get; set; }


        public static explicit operator ApplicationWorker(Users user) {
            return new ApplicationWorker {
                Name = user.Name,
                UserLogoPath = user.UserLogoPath,
                Rating = user.Rating,
                PublicKey = user.PublicKey ?? Guid.Empty
            };
        }
    }
}
