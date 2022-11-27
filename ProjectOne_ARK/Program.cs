using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectOne_ARK
{
    class Program
    {
        private const string JSON_FILE = @"Data/Canadacities-JSON.json";
        // XML Data
        private const string XML_FILE = @"Data/Canadacities-XML.xml";
        // CSV Data
        private const string CSV_FILE = @"Data/Canadacities.csv";
        static void Main(string[] args)
        {
            //Set some variables to allow for user input/validation
            string fileParseSelection;
            string userSelection;
            Dictionary<string, List<CityInfo>> cityInformation = new Dictionary<string, List<CityInfo>>();
            CityInfo firstCity = new CityInfo();
            CityInfo secondCity = new CityInfo();
            DataModeler fileParser = new DataModeler();
            bool switchUserInputController = false;

            Statistics cityStatsJSON = new Statistics(JSON_FILE, "json");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome to the City Statistics Program Brought to you by ARK");
            Console.WriteLine("------------------------------------------------------------\n");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("To begin, choose which kind file you would like to parse\nTo exit, type EXIT");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("1.) JSON");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("2.) CSV ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("3.) XML");
            Console.ResetColor();
            Console.WriteLine();
            fileParseSelection = Console.ReadLine();
            do
            {
                switch (fileParseSelection)
                {
                    case "1":
                        {
                            Console.WriteLine("Parsing JSON file..");
                            cityInformation = fileParser.ParseFile(JSON_FILE, "json");
                            switchUserInputController = true;
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("Parsing CSV..");
                            cityInformation = fileParser.ParseFile(CSV_FILE, "csv");
                            switchUserInputController = true;
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Parsing XML..");
                            cityInformation = fileParser.ParseFile(XML_FILE, "xml");
                            switchUserInputController = true;
                            break;
                        }
                    case "exit":
                        {
                            break;
                        }
                }
            } while (!switchUserInputController);
            do
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPlease choose which action to take on the data: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1.) Display the city information");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("2.) Display cities within a province");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("3.) Display city with the largest population by province");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("4.) Display city with the smallest population by province");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("5.) Compare the population of two cities");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("6.) Display Province population");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("7.) Get the Capital of a Province");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("8.) Rank provinces by population (smallest to largest)");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("9.) Rank provinces by number of cities (smallest to largest)");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("10.) Show City on Map");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("11.) Calculate the distance between two cities");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("OR Type exit to exit");
                Console.ResetColor();
                userSelection = Console.ReadLine();
                Console.ResetColor();
                if (userSelection == "1")
                {
                    string cityName;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("Which city would you like to see information for?");
                    Console.WriteLine("-------------------------------------------------");
                    Console.ResetColor();
                    cityName = Console.ReadLine();
                    foreach (var city in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in city)
                        {
                            if (cityObjectparse.City == cityName)
                            {
                                var foundCityData = cityStatsJSON.DisplayCityInformation(cityName);
                                foreach (var cityResults in foundCityData)
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("City Name: " + cityResults.City + "\nProvince: " + cityResults.Admin_Name + "\nCountry: " +
                                        cityResults.Country + "\nLatitude: " + cityResults.Lat + "\nLongitude: " + cityResults.Lng + "\nPopulation : "
                                        + cityResults.Population + "\nCapital: " + cityResults.Capital);
                                    Console.ResetColor();
                                }
                                break;
                            }
                        }
                    }
                }
                if (userSelection == "2")
                {
                    string provinceName;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Which province would you like to see information for?");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.ResetColor();
                    provinceName = Console.ReadLine();
                    //checking for valid user input
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Here are all the cities in " + provinceName);
                    Console.WriteLine("---------------------------------------------");
                    Console.ResetColor();
                    var foundCityData = cityStatsJSON.DisplayProvinceCities(provinceName);
                    foreach (var provResults in foundCityData)
                    {
                       Console.BackgroundColor = ConsoleColor.Black;
                       Console.ForegroundColor = ConsoleColor.DarkYellow;
                       Console.Write(String.Format("{0,40}", provResults.City));
                       Console.ResetColor();
                    }
                }
                if (userSelection == "3")
                {
                    string cityPop;
                    CityInfo foundCityData = new CityInfo();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("Choose a province to return its city with the largest population");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.ResetColor();
                    cityPop = Console.ReadLine();
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.Admin_Name == cityPop)
                            {
                                foundCityData = cityStatsJSON.DisplayLargestCityPopulation(cityPop);
                                break;
                            }
                        }
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("The city with the highest population in " + cityPop + " is: ");
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(String.Format("{0} {1:#,###,###,###}", foundCityData.City, foundCityData.Population));
                    Console.ResetColor();
                }
                if (userSelection == "4")
                {
                    string smallPop;
                    CityInfo foundCityData = new CityInfo();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.WriteLine("Choose a province to return its city with the smallest population");
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.ResetColor();
                    smallPop = Console.ReadLine();
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.Admin_Name == smallPop)
                            {
                                foundCityData = cityStatsJSON.DisplaySmallestCityPopulation(smallPop);
                                break;
                            }
                        }
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("The city with the smallest population in " + smallPop + " is :");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(String.Format("{0} {1:#,###,###,###}", foundCityData.City, foundCityData.Population));
                    Console.ResetColor();
                }
                if (userSelection == "5")
                {
                    string cityOne;
                    string cityTwo;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Compare the population of two cities:                ");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Enter first city: ");
                    cityOne = Console.ReadLine();
                    Console.WriteLine("Enter second city: ");
                    cityTwo = Console.ReadLine();
                    Console.ResetColor();
                    //End beginning prompt text
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.City == cityOne)
                            {
                                var foundCityOne = cityStatsJSON.DisplayCityInformation(cityOne);
                                foreach (var city in foundCityOne)
                                {
                                    firstCity.City = city.City;
                                    firstCity.Admin_Name = city.Admin_Name;
                                    firstCity.Country = city.Country;
                                    firstCity.Lat = city.Lat;
                                    firstCity.Lng = city.Lng;
                                    firstCity.Population = city.Population;
                                    firstCity.Capital = city.Capital;
                                }
                            }
                        }
                    }
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.City == cityTwo)
                            {
                                var foundCityTwo = cityStatsJSON.DisplayCityInformation(cityTwo);
                                foreach (var city2 in foundCityTwo)
                                {
                                    secondCity.City = city2.City;
                                    secondCity.Admin_Name = city2.Admin_Name;
                                    secondCity.Country = city2.Country;
                                    secondCity.Lat = city2.Lat;
                                    secondCity.Lng = city2.Lng;
                                    secondCity.Population = city2.Population;
                                    secondCity.Capital = city2.Capital;
                                }
                            }
                        }
                    }
                    var findHighPop = cityStatsJSON.CompareCitiesPopulation(firstCity, secondCity);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("The city with the highest population : \n");
                    Console.WriteLine("-----------------------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(String.Format("{0:#,###,###}", findHighPop));
                    Console.ResetColor();
                }
                if (userSelection == "6")
                {
                    string provinceName;
                    float foundCityData = 0;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Which province would you like to see information for?");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.ResetColor();
                    provinceName = Console.ReadLine();
                    //checking for valid user input
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.Admin_Name == provinceName)
                            {
                                foundCityData = cityStatsJSON.DisplayProvincePopulation(provinceName);
                                break;
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Here is the total population for: " + provinceName);
                    Console.WriteLine("--------------------------------------------------");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(String.Format("{0:#,###,###}", foundCityData));
                    Console.ResetColor();
                }
                if (userSelection == "7")
                {
                    //Get the capital of a province
                    string provinceName;
                    string foundCityData = "";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Which province would you like to see the Capital for?");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.ResetColor();
                    provinceName = Console.ReadLine();
                    //checking for valid user input
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.Admin_Name == provinceName)
                            {
                                foundCityData = cityStatsJSON.GetCapital(provinceName);
                                break;
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("The capital of " + provinceName + " is:       ");
                    Console.WriteLine("---------------------------------------------");
                    Console.ResetColor();
                    Console.WriteLine(foundCityData);

                }
                if (userSelection == "8")
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Provinces Ranked from smallest to largest by population");
                    Console.WriteLine("-------------------------------------------------------");
                    Console.ResetColor();
                    var foundCityData = cityStatsJSON.RankProvincesByPopulation();
                    Console.WriteLine(foundCityData);
                }
                if (userSelection == "9")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Provinces Ranked from smallest to largest by number of cities");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.ResetColor();
                    var foundCityData = cityStatsJSON.RankProvincesByCities();
                    Console.WriteLine(foundCityData);
                }
                if (userSelection == "10")
                {
                    string cityforMap;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Which city would you like to see on the map?           ");
                    Console.WriteLine("-------------------------------------------------------");
                    Console.ResetColor();
                    cityforMap = Console.ReadLine();
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.City == cityforMap)
                            {
                                var foundCityOne = cityStatsJSON.DisplayCityInformation(cityforMap);
                                foreach (var city in foundCityOne)
                                {
                                    firstCity.City = city.City;
                                    firstCity.Admin_Name = city.Admin_Name;
                                    firstCity.Country = city.Country;
                                    firstCity.Lat = city.Lat;
                                    firstCity.Lng = city.Lng;
                                    firstCity.Population = city.Population;
                                    firstCity.Capital = city.Capital;
                                }
                            }
                        }
                    }
                    cityStatsJSON.ShowCityOnMap(firstCity);           
                }
                if (userSelection == "11")
                {
                    //Calculate the distance between two cities 
                    string cityOne;
                    string cityTwo;
                    double foundCityData = 0;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Find the distance between two cities:                ");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Enter first city: ");
                    cityOne = Console.ReadLine();
                    Console.WriteLine("Enter second city: ");
                    cityTwo = Console.ReadLine();
                    Console.ResetColor();
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.City == cityOne)
                            {
                                var foundCityOne = cityStatsJSON.DisplayCityInformation(cityOne);
                                foreach (var city in foundCityOne)
                                {
                                    firstCity.City = city.City;
                                    firstCity.Admin_Name = city.Admin_Name;
                                    firstCity.Country = city.Country;
                                    firstCity.Lat = city.Lat;
                                    firstCity.Lng = city.Lng;
                                    firstCity.Population = city.Population;
                                    firstCity.Capital = city.Capital;
                                }
                            }
                        }
                    }
                    foreach (var prov in cityInformation.Values)
                    {
                        foreach (var cityObjectparse in prov)
                        {
                            if (cityObjectparse.City == cityTwo)
                            {
                                var foundCityTwo = cityStatsJSON.DisplayCityInformation(cityTwo);
                                foreach (var city2 in foundCityTwo)
                                {
                                    secondCity.City = city2.City;
                                    secondCity.Admin_Name = city2.Admin_Name;
                                    secondCity.Country = city2.Country;
                                    secondCity.Lat = city2.Lat;
                                    secondCity.Lng = city2.Lng;
                                    secondCity.Population = city2.Population;
                                    secondCity.Capital = city2.Capital;
                                }
                            }
                        }
                    }
                    foundCityData = cityStatsJSON.CalculateDistanceBetweenCities(firstCity, secondCity);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("The Distance between " + cityOne + " and " + cityTwo + " is:");
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(foundCityData);
                    Console.ResetColor();
                }
            } while (userSelection.ToLower() != "exit");
            if (userSelection.ToLower() == "exit")
            {
                System.Environment.Exit(0);
            }
        }
    }
}

