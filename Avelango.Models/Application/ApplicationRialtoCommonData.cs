using System.Collections.Generic;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationRialtoState
    {
        public double AveRate { get; set; }
        public double TotalAveCount { get; set; }
        public double BenefitPersent { get; set; }
        public ApplicationRialtoUsersStates UserState { get; set; }
        public List<ApplicationRialtoUserAssets> Assets { get; set; }


        public static explicit operator ApplicationRialtoState(RialtoState rialtoState) {
            return new ApplicationRialtoState {
                AveRate = rialtoState.AveRate,
                TotalAveCount = rialtoState.TotalAveCount,
                BenefitPersent = rialtoState.BenefitPersent
            };
        }
    }
}
