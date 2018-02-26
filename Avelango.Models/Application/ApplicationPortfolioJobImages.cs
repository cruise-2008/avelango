using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationPortfolioJobImages
    {
        public string Small { get; set; }
        public string Large { get; set; }


        public static explicit operator ApplicationPortfolioJobImages(PortfolioJobImages portfolioJobImages) {
            return new ApplicationPortfolioJobImages {
                Small = portfolioJobImages.Small,
                Large = portfolioJobImages.Large,
            };
        }
    }
}
