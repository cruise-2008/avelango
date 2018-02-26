using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Enums;

namespace Avelango.Handlers.Lang
{
    public static class LocationManager
    {
        //public static readonly List<Country> LocationEn = GetLocationsList(Langs.LangsEnum.En.ToString());
        //public static readonly List<Country> LocationRu = GetLocationsList(Langs.LangsEnum.Ru.ToString());
        //public static readonly List<Country> LocationDe = GetLocationsList(Langs.LangsEnum.De.ToString());
        //public static readonly List<Country> LocationEs = GetLocationsList(Langs.LangsEnum.Es.ToString());
        //public static readonly List<Country> LocationFr = GetLocationsList(Langs.LangsEnum.Fr.ToString());
        //public static readonly List<Country> LocationUa = GetLocationsList(Langs.LangsEnum.Ua.ToString());
        //
        //
        //public static List<Country> GetLocations(string lang) {
        //    try {
        //        switch (lang.ToLower().Trim()) {
        //            case "ru": { return LocationRu; }
        //            case "fr": { return LocationFr; }
        //            case "de": { return LocationDe; }
        //            case "es": { return LocationEs; }
        //            case "ua": { return LocationUa; }
        //            default: { return LocationEn; }
        //        }
        //    }
        //    catch { return LocationEn; }
        //}
        //
        //
        //private static List<Country> GetLocationsList(string lang) {
        //    var locations = new List<Country>();
        //    var countries = PageLangManager.GetCountries(lang).OrderBy(x => x.Value);
        //    var cities = PageLangManager.GetCities(lang).OrderBy(x => x.Value);
        //    foreach (var country in countries) {
        //        var contr = new Country {
        //            Code = country.Key,
        //            Cities = new List<City>()
        //        };
        //        foreach (var city in cities) {
        //            if (city.Key.Contains(country.Key)) {
        //                contr.Cities.Add(new City {
        //                    Code = city.Key,
        //                });
        //            }
        //        }
        //        locations.Add(contr);
        //    }
        //    return locations;
        //}
    }
}