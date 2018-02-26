
using System.Collections.Generic;

namespace Avelango.Handlers.Lang
{
    public class Country {
        public string Code { get; set; }
        public List<City> Cities { get; set; }
    }


    public class City {
        public string Code { get; set; }
    }
}
