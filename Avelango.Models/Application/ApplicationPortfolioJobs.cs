using System.Collections.Generic;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationPortfolioJobs
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ApplicationPortfolioJobImages> Images { get; set; }


        public static explicit operator ApplicationPortfolioJobs(PortfolioJobs portfolioJobs)
        {
            return new ApplicationPortfolioJobs {
                Title = portfolioJobs.Title,
                Description = portfolioJobs.Description,
            };
        }
    }
}
