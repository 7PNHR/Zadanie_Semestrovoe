namespace Zadanie_Semestrovoe
{
    public class Road 
    {
        public City From;
        public City To;
        
        public int TimeInMinutes;
        
        public Road(City from, City to, int timeInMinutes)
        {
            From = from;
            To = to;
            TimeInMinutes = timeInMinutes;
        }
    }
}