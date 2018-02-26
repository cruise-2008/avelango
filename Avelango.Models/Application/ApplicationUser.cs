using System;
using System.Collections.Generic;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationUser : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public Guid Pk { get; set; }
        public bool IsCompany { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Skype { get; set; }
        public string Ballance { get; set; }
        public string Valuta { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime? LastLogIn { get; set; }
        public string UserLogoPath { get; set; }
        public string UserLogoPathMax { get; set; }
        public bool? Gender { get; set; }
        public string Birthday { get; set; }
        public double? Rating { get; set; }
        public string Honors { get; set; }
        public string SubscribeToGroups { get; set; }
        public bool IsEnabled { get; set; }
        public bool ReceiveNews { get; set; }
        public string Lang { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsActive { get; set; }
        public bool EmailDelivery { get; set; }
        public bool SmsDelivery { get; set; }
        public bool PushUpDelivery { get; set; }

        public string PlaceName { get; set; }
        public double? PlaceLat { get; set; }
        public double? PlaceLng { get; set; }


        public ApplicationPortfolio Portfolio { get; set; }
        public List<ApplicationComments> Comments { get; set; }
       


        public static explicit operator ApplicationUser(Users user) {
            return new ApplicationUser {
                Pk = user.PublicKey ?? Guid.Empty,
                Id = user.PublicKey.ToString(),
                Email = user.Email,
                IsAdmin = user.IsAdmin ?? false,
                IsModerator = user.IsModerator ?? false,
                IsEnabled = user.IsEnabled,
                Lang = user.Lang,
                ReceiveNews = user.ReceiveNews,
                SubscribeToGroups = user.SubscribeToGroups,
                Honors = user.Honors,
                Rating = user.Rating,
                Birthday = user.Birthday,
                Gender = user.Gender,
                UserLogoPath = user.UserLogoPath,
                UserLogoPathMax = user.UserLogoPathMax,
                LastLogIn = user.LastLogIn,
                AccountCreated = user.AccountCreated,
                Valuta = user.Valuta,
                Ballance = user.Ballance,
                Skype = user.Skype,
                Phone1 = user.Phone1,
                Phone2 = user.Phone2,
                Phone3 = user.Phone3,
                IsCompany = user.IsCompany,
                Name = user.Name,
                SurName = user.SurName,
                IsActive = user.IsActive,
                EmailDelivery = user.EmailDelivery ?? false,
                SmsDelivery = user.SmsDelivery ?? false,
                PushUpDelivery = user.PushUpDelivery ?? false,
                Portfolio = null,
                PlaceName = user.PlaceName,
                PlaceLat = user.PlaceLat,
                PlaceLng = user.PlaceLng,
            };
        }
    }
}
