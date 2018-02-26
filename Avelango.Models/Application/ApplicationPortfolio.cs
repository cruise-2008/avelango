using System.Collections.Generic;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationPortfolio
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ApplicationPortfolioJobs> Jobs { get; set; }


        public static explicit operator ApplicationPortfolio(Portfolios portfolio) {
            var appPortfolio = new ApplicationPortfolio {Jobs = new List<ApplicationPortfolioJobs>()};
            if (portfolio == null) return appPortfolio;
            appPortfolio.Title = portfolio.Title;
            appPortfolio.Description = portfolio.Description;
            return appPortfolio;
        }
    }
}
