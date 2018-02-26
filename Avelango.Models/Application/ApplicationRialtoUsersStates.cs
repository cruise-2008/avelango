using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationRialtoUsersStates
    {
        public double Ballance { get; set; }
        public double Equity { get; set; }


        public static explicit operator ApplicationRialtoUsersStates(RialtoUsersStates rialtoUserState) {
            return new ApplicationRialtoUsersStates {
                Ballance = rialtoUserState.Ballance,
                Equity = rialtoUserState.Equity
            };
        }
    }
}
