using System;
using System.Collections.Generic;
using System.Linq;

namespace Zadanie_Semestrovoe
{
    public class Map
    {
        private readonly HashSet<City> _cities = new HashSet<City>();
        
        public void AddCity(string cityName) => _cities.Add(new City(cityName));
        
        private City FindCity(string name) => _cities.FirstOrDefault(x => x.Name == name);
        

        public void RemoveCity(string cityName)
        {
            var city = FindCity(cityName);
            if (city == null) throw new Exception("Такого города нет!");
            foreach (var incidentCity in city.IncidentCities.Values)
            {
                Disconnect(city,incidentCity);
                incidentCity.IncidentCities.Remove(cityName);
            }

            _cities.Remove(city);
        }
        
        public void Connect(string firstCityName, string secondCityName,int time)
        {
            var (firstCity, secondCity) = GetCities(firstCityName, secondCityName);
            if(firstCity.IncidentCities.ContainsValue(secondCity) || secondCity.IncidentCities.ContainsValue(firstCity) )
                throw new Exception("Города уже связаны!");
            firstCity.Connect(secondCity,time);
        }
        
        public void Disconnect(string firstCityName, string secondCityName)
        {
            var (firstCity, secondCity) = GetCities(firstCityName, secondCityName);
            Disconnect(firstCity,secondCity);
        }
        
        private void Disconnect(City firstCity, City secondCity) => firstCity.Disconnect(secondCity);
        
        private Tuple<City,City> GetCities(string firstCityName, string secondCityName)
        {
            if(firstCityName==secondCityName)
                throw new Exception("Города совпадают!");
            var firstCity = FindCity(firstCityName);
            var secondCity = FindCity(secondCityName);
            return Tuple.Create(firstCity, secondCity);
        }

        private void Check()
        {
            foreach (var city in _cities)
                if(city.Roads.Count<2)
                    throw new Exception("Невозможно построить путь, так как есть города с 0 или 1 дорогой!!!");
        }

        public Tuple<List<City>, int> GeTMinWayByBruteForce(string cityName)
        {
            Check();
            var city = FindCity(cityName);
            var roads = new Dictionary<int,List<City>>();
            var ways = new List<List<City>>();
            FindByBruteForce(city, city, new List<City>(), ways);
            foreach (var road in ways)
                roads[GetRoadTime(road)] = road;
            return roads
                .Where(x => x.Key == roads.Keys.Min())
                .Select(x => Tuple.Create(x.Value, x.Key))
                .First();
        }

        private void FindByBruteForce(City firstCity,City currentCity, List<City> visitedCities,List<List<City>> roads)
        {
            foreach (var incidentCity in currentCity.IncidentCities.Values)
            {
                if(visitedCities.Contains(incidentCity)) continue;
                var newVisitedCities = new List<City>(visitedCities) {currentCity};
                FindByBruteForce(firstCity, incidentCity, newVisitedCities,roads);
            }

            visitedCities.Add(currentCity);
            if (currentCity.IncidentCities.ContainsValue(firstCity) && visitedCities.Count == _cities.Count)
            {
                visitedCities.Add(firstCity);
                roads.Add(visitedCities);
            }
        }

        private int GetRoadTime(List<City> cities)
        {
            var sum = 0;
            for (int i = 0; i < cities.Count-1; i++)
            {
                var road = cities[i].FindRoad(cities[i + 1]);
                sum += road.TimeInMinutes;
            }

            return sum;
        }

        public Tuple<List<City>, int> GeTMinWayByCuttingOffPartsOfRoads(string cityName)
        {
            Check();
            var city = FindCity(cityName);
            var value = int.MaxValue;
            var roads = new Dictionary<int,List<City>>();
            var time = FindByCuttingOffPartsOfRoads(city, city, new List<City>(), roads, int.MaxValue, 0);
            var fastWay = roads[time];
            return Tuple.Create(fastWay,time);
        }

        private int FindByCuttingOffPartsOfRoads(City firstCity,City city,List<City> visitedCities,Dictionary<int,List<City>> roads,int minTime,int time)
        {
            foreach (var incidentCity in city.IncidentCities.Values)
            {
                var newTime = time;
                if(visitedCities.Contains(incidentCity)) continue;
                newTime += GetTime(city, incidentCity);
                if(minTime<newTime) continue;
                var newVisitedCities = new List<City>(visitedCities) {city};
                var t = FindByCuttingOffPartsOfRoads(firstCity, incidentCity, newVisitedCities, roads, minTime, newTime);
                if (t <= minTime) minTime = t;
            }

            visitedCities.Add(city);
            if (city.IncidentCities.ContainsValue(firstCity) && visitedCities.Count == _cities.Count)
            {
                visitedCities.Add(firstCity);
                time += GetTime(city, firstCity);
                roads[time] = visitedCities;
                return time;
            }

            return minTime;
        }
        
        private int GetTime(City firstCity, City secondCity) 
            => firstCity.Roads.Find(road => road.To == secondCity).TimeInMinutes;
    }
}



























