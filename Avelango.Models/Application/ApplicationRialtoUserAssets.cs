using System;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationRialtoUserAssets
    {
        public Guid? Pk { get; set; }
        public double Value { get; set; }
        public double Benefit { get; set; }
        public double Rate { get; set; }
        public bool Sign { get; set; }


        public static explicit operator ApplicationRialtoUserAssets(RialtoUserAssets rialtoUserAsset) {
            return new ApplicationRialtoUserAssets {
                Pk = rialtoUserAsset.PublicKey,
                Value = rialtoUserAsset.Value,
                Benefit = rialtoUserAsset.Benefit,
                Rate = rialtoUserAsset.Rate,
                Sign = rialtoUserAsset.Sign,
            };
        }
    }
}
