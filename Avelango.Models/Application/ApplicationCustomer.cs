using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationCustomer
    {
        public string Pk { get; set; }
        public string Name { get; set; }
        public string UserLogoPath { get; set; }
        public double? Rating { get; set; }


        public static explicit operator ApplicationCustomer(Users user) {
            return new ApplicationCustomer {
                Pk = user.PublicKey.ToString(),
                Name = user.Name,
                UserLogoPath = string.IsNullOrEmpty(user.UserLogoPath) ? Resources.Default.Settings.DefaulLogoPath : user.UserLogoPath,
                Rating = user.Rating,
            };
        }
    }
}
