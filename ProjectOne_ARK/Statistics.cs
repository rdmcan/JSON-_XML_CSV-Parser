using System.Collections.Generic;
using System;

namespace ProjectOne_ARK
{
    internal class Statistics
    {
        internal List<string> provinceList = new List<string>();
        internal Dictionary<string, List<CityInfo>> CityCatalogue = new Dictionary<string, List<CityInfo>>();
        public string filename = "";
        public string extension = "";
        public const double EarthRadius = 6371;


        //2 arg constructor
        public Statistics(string filename, string filetype)
        {
            this.filename = filename;
            this.extension = filetype;
            DataModeler dm = new DataModeler();
            CityCatalogue = dm.ParseFile(filename, filetype);

            //fills the province list for the class
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (CityInfo cityItem in cityList.Value)
                {
                    if (!provinceList.Contains(cityItem.Admin_Name))
                        provinceList.Add(cityItem.Admin_Name);
                }
            }
        }//end of Statistics

        //
        //Start of City Methods
        //

        //Displays the cities information
        public List<CityInfo> DisplayCityInformation(string cityName)
        {
            List<CityInfo> returnCities = new List<CityInfo>();
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (CityInfo cityItem in cityList.Value)
                {
                    if (cityItem.City == cityName)
                    {
                        returnCities.Add(cityItem);
                    }
                }
            }
            return returnCities;
        }//end of DisplayCityInformation

        //displays the largest population
        public CityInfo DisplayLargestCityPopulation(string province)
        {
            float highestPop = 0;
            //should be fine but if it has issues just change to select the first on and create it with that
            CityInfo returnCity = new CityInfo();
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (var cityItem in cityList.Value)
                {
                    if (cityItem.Admin_Name == province && cityItem.Population > highestPop)
                    {
                        highestPop = cityItem.Population;
                        returnCity.City = cityItem.City;
                        returnCity.Admin_Name = cityItem.Admin_Name;
                        returnCity.Capital = cityItem.Capital;
                        returnCity.Lat = cityItem.Lat;
                        returnCity.Lng = cityItem.Lng;
                        returnCity.Population = cityItem.Population;
                    }
                }
            }
            return returnCity;
        }//end of DisplayLargestCityPopulation

        //Displays the smallest population
        public CityInfo DisplaySmallestCityPopulation(string province)
        {
            float lowestPop = float.MaxValue;//set to the largest possible value
            CityInfo returnCity = new CityInfo(); //should be fine but if it has issues just change to select the first on and create it with that
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (var cityItem in cityList.Value)
                {
                    if (cityItem.Admin_Name == province && cityItem.Population < lowestPop)
                    {
                        lowestPop = cityItem.Population;
                        returnCity.City = cityItem.City;
                        returnCity.Admin_Name = cityItem.Admin_Name;
                        returnCity.Capital = cityItem.Capital;
                        returnCity.Lat = cityItem.Lat;
                        returnCity.Lng = cityItem.Lng;
                        returnCity.Population = cityItem.Population;
                    }
                }
            }
            return returnCity;
        }// end of DisplaySmallestCityPopulation

        //compares two cities and determines which has the greater population
        public string CompareCitiesPopulation(CityInfo city1, CityInfo city2)
        {
            string returnValue = "";
            if (city1.Population > city2.Population)
                returnValue = String.Format("{0} {1} {2:#,###,###} {3} {4} {5} {6:#,###,###} ", city1.City, " has the higher population at ", city1.Population, " compared to ", city2.City, " at ", city2.Population);
            else
                returnValue = String.Format("{0} {1} {2:#,###,###} {3} {4} {5} {6:#,###,###}", city2.City, " has the higher population at ", city2.Population, " compared to ", city1.City, " at ", city1.Population);
            return returnValue;
        }//end of CompareCitiesPopulation

        //opens web browser with the selected city on a map
        public void ShowCityOnMap(CityInfo city)
        {
            //create the target url to launch
            string target = "https://www.latlong.net/c/?lat=" + city.Lat + "&long=" + city.Lng;

            //try to open the website
            try
            {
                System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", target);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    Console.WriteLine("Google chrome is located in a differant location or outdated");
                    Console.WriteLine(noBrowser.Message);
                }
            }
        }//end of Show on Map

        //calculates and returns the distance in km between city1 and city2
        public double CalculateDistanceBetweenCities(CityInfo city1, CityInfo city2)
        {
            double distance = 0.0;
            //uses the mathimatical formula to calculate distance around the earth
            double Lat = (city2.Lat - city1.Lat) * (Math.PI / 180);
            double Lon = (city2.Lng - city1.Lng) * (Math.PI / 180);
            double a = Math.Sin(Lat / 2) * Math.Sin(Lat / 2) + Math.Cos(city1.Lat * (Math.PI / 180)) * Math.Cos(city2.Lat * (Math.PI / 180)) * Math.Sin(Lon / 2) * Math.Sin(Lon / 2);
            double b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            distance = EarthRadius * b;
            return distance;
        }//end of CalculateDistanceBetweenCities

        //End of City Methods
        //
        //Start of Provinces Methods

        //Display the total population of a province
        public float DisplayProvincePopulation(string province)
        {
            //varaible to store total provincePopulation
            float provincePopulation = 0;
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (var cityItem in cityList.Value)
                {
                    //foreach city in the province add the cities population to the return value
                    if (cityItem.Admin_Name == province)
                    {
                        provincePopulation += cityItem.Population;
                    }
                }
            }
            return provincePopulation;
        }//end of DisplayProvincePopulation

        //inputs a province name and grabs all cities in the province to be displayed 
        public List<CityInfo> DisplayProvinceCities(string province)
        {
            //create a list to store the cities
            List<CityInfo> returnCityList = new List<CityInfo>();

            //foreach city in the cityCatalogue
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            { 
                foreach (CityInfo cityItem in cityList.Value)
                {
                    //if the province is correct add the cityInfo object to the returnCityList
                    if (cityItem.Admin_Name == province)
                    {
                        returnCityList.Add(cityItem);
                    }
                }
            }

            return returnCityList;
        }//end of DisplayProvinceCities

        //Ranks the provinces by total population
        public string RankProvincesByPopulation()
        {
            //create a Dictionary to contain the province name / the population
            Dictionary<string, float> provincePop = new Dictionary<string, float>();
            string returnValue = "";

            //foreach province in provinceList add to provincePop (Fill the dictionary)
            foreach (string province in provinceList)
            {
                provincePop.Add(province, 0);
            }


            //foreach province add up the total population
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (CityInfo cityItem in cityList.Value)
                {
                    provincePop[cityItem.Admin_Name] += cityItem.Population;
                }
            }

            //create list to sort population number
            List<float> popList = new List<float>();
            foreach (KeyValuePair<string, float> province in provincePop)
            {
                popList.Add(province.Value);
            }

            //sort the  popList so it can reference the provincePop by value
            float tmp = 0;
            for (int i = 0; i < popList.Count; i++)
                for (int x = 0; x < (popList.Count - 1); x++)
                    if (popList[x] > popList[x + 1])
                    { //Simple bubble sort
                        tmp = popList[x];
                        popList[x] = popList[x + 1];
                        popList[x + 1] = tmp;
                    }

            //for each value in popList find the matching province in provincePop
            foreach (float value in popList)
            {
                foreach (KeyValuePair<string, float> province in provincePop)
                {
                    //if the values equal then at the return value to the string. this will be in ascending order
                    if (province.Value == value)
                    {
                        returnValue += province.Key + " has a population of " + value + "\n";
                    }
                }
            }

            //will return a formated string for output
            return returnValue;
        }//end of RankProvincesByPopulation

        public string RankProvincesByCities()
        {
            Dictionary<string, int> provinceCities = new Dictionary<string, int>();
            string returnValue = "";

            //foreach province in provinceList add to provinceCities
            foreach (string province in provinceList)
            {
                provinceCities.Add(province, 0);
            }


            //foreach province add up the total number of cities
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (CityInfo cityItem in cityList.Value)
                {
                    provinceCities[cityItem.Admin_Name]++;
                }
            }

            //create list to sort cities number
            List<int> cities = new List<int>();
            foreach (KeyValuePair<string, int> province in provinceCities)
            {
                cities.Add(province.Value);
            }

            //sort the  cities list so it can reference the provinceCities by value
            int tmp = 0;
            for (int i = 0; i < cities.Count; i++)
                for (int x = 0; x < (cities.Count - 1); x++)
                    if (cities[x] > cities[x + 1])
                    { //Simple bubble sort
                        tmp = cities[x];
                        cities[x] = cities[x + 1];
                        cities[x + 1] = tmp;
                    }

            //for each value in cities find the matching province in provinceCities
            foreach (int value in cities)
            {
                foreach (KeyValuePair<string, int> province in provinceCities)
                {
                    //if the values equal then at the return value to the string. this will be in ascending order
                    if (province.Value == value)
                    {
                        returnValue += province.Key + " has a total number of " + value + " Cities \n";
                    }
                }
            }

            //will return a formated string for output
            return returnValue;
        }//end of RankProvincesByCities
        
        //get the capital for a specific province
        public string GetCapital(string province)
        {
            //foreach city in the CityCatalogue
            foreach (KeyValuePair<string, List<CityInfo>> cityList in CityCatalogue)
            {
                foreach (CityInfo cityItem in cityList.Value)
                {
                    //if there province is correct and there capital is not empty
                    if (cityItem.Admin_Name == province && cityItem.Capital != "")
                    {
                        //if the capital field == admin then add to the returnValue 
                        if (cityItem.Capital == "admin")
                        {
                            return "The capital of " + cityItem.Admin_Name + " is " + cityItem.City + " at " + cityItem.Lat + " lattitude and " + cityItem.Lng + " longitude.";
                        }

                    }
                }
            }
            return "";
        }
    }
}