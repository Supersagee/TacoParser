using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            var lines = File.ReadAllLines(csvPath);

            logger.LogInfo($"Lines: {lines[0]}");

            var parser = new TacoParser();

            var locations = lines.Select(parser.Parse).ToArray();

            ITrackable tacoBell1 = null;
            ITrackable tacoBell2 = null;
            double distance = 0;

            for (var i = 0; i < locations.Length; i++)
            {
                logger.LogInfo($"Parsing the list ---");
                
                var locationA = locations[i];
                
                var coordinateA = new GeoCoordinate();
                coordinateA.Latitude = locationA.Location.Latitude;
                coordinateA.Longitude = locationA.Location.Longitude;

                for (var j = 0; j < locations.Length; j++)
                {
                    var locationB = locations[j];
                    
                    var coordinateB = new GeoCoordinate();
                    coordinateB.Latitude = locationB.Location.Latitude;
                    coordinateB.Longitude = locationB.Location.Longitude;

                    if (coordinateA.GetDistanceTo(coordinateB) > distance)
                    {
                        distance = coordinateA.GetDistanceTo(coordinateB);
                        tacoBell1 = locationA;
                        tacoBell2 = locationB;
                    }

                }
                
            }

            logger.LogInfo($"{tacoBell1.Name} and {tacoBell2.Name} are the farthest apart");
        }
    }
}
