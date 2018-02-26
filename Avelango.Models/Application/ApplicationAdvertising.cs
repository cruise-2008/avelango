using System;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationAdvertising
    {
        public Guid PublicKey { get; set; }
        public string CompanyName { get; set; }
        public int CompanyRate { get; set; }
        public DateTime Expired { get; set; }
        public string ImagePath { get; set; }
        public string FirstData { get; set; }
        public string SecondData { get; set; }
        public string Icon { get; set; }


        public static explicit operator ApplicationAdvertising(CommonAdvertising advertising) {
            return new ApplicationAdvertising {
                PublicKey = advertising.PublicKey,
                CompanyName = advertising.CompanyName,
                CompanyRate = advertising.CompanyRate,
                Expired = advertising.Expired,
                ImagePath = advertising.ImagePath,
                FirstData = advertising.FirstData,
                SecondData = advertising.SecondData,
                Icon = advertising.Icon
            };
        }
    }
}
