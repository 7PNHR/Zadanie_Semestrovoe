using System.Collections.Generic;

namespace Zadanie_Semestrovoe
{
    public class City
    {
        public string Name;
        public Dictionary<string,City> IncidentCities = new Dictionary<string,City>();
        public List<Road> Roads = new List<Road>();

        public City(string name) => Name = name;

        public override string ToString() => Name;

        public void Connect(City anotherCity, int time)
        {
            IncidentCities[anotherCity.Name] = anotherCity;
            anotherCity.IncidentCities[Name] = this;
            Roads.Add(new Road(this,anotherCity,time));
            anotherCity.Roads.Add(new Road(anotherCity, this,time));
        }
        
        public void Disconnect(City anotherCity)
        {
            IncidentCities.Remove(anotherCity.Name);
            anotherCity.IncidentCities.Remove(Name);
            Roads.Remove(Roads.Find(x => x.To == anotherCity));
            anotherCity.Roads.Remove(anotherCity.Roads.Find(x => x.To == this));
        }

        public Road FindRoad(City anotherCity)
        {
            foreach (var road in Roads)
                if (road.From == this && road.To == anotherCity)
                    return road;
            return null;
        }
    }
}