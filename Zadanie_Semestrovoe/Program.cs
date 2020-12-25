using System.Diagnostics;

namespace Zadanie_Semestrovoe
{
    class Program
    {
        static void Main()
        {
            var map = Uploader.UploadCitiesAndRoads("Города.txt","Дороги.txt");
            var time = new Stopwatch();
            time.Start();
            var minRoadFoundedByBruteForce = map.GeTMinWayByBruteForce("Самара");
            time.Stop();
            var firstTime = time.Elapsed;
            time.Reset();
            time.Start();
            var minRoadFoundedByCuttingOffPartsOfRoads = map.GeTMinWayByCuttingOffPartsOfRoads("Самара");
            time.Stop();
            var secondTime = time.Elapsed;
        }
    }
}