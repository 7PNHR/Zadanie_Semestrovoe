using System.IO;
using System.Linq;

namespace Zadanie_Semestrovoe
{
    public static class Uploader
    {
        public static Map UploadCities(string citiesName)
        {
            var map = new Map(); 
            var cities = File.ReadAllText(citiesName)
                .Split('\r','\n')
                .Where(x=> x!="")
                .ToList();
            foreach (var city in cities)
                map.AddCity(city);
            return map;
        }

        public static Map UploadCitiesAndRoads(string citiesName, string roadsName)
        {
            var map = UploadCities(citiesName);
            var roads = File.ReadAllText(roadsName)
                .Split('\r','\n')
                .Where(x=> x!="")
                .ToList();
            foreach (var road in roads)
            {
                var line = road.Split(' ');
                map.Connect(line[0],line[1],int.Parse(line[2]));
            }

            return map;
        }
    }
}