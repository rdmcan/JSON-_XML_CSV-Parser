namespace ProjectOne_ARK
{
   public class CityInfo 
    {
        //create City info properties
        //properties are created in a format to support json parsing
        public int Id { get; set; }
        public string City { get; set; }
        public float Population;
        public string Province;
        public double Lng;
        public double Lat;
        public string Country { get; set; }
        public string City_Ascii { get; set; }
        public string Admin_Name { get; set; }
        public string Capital { get; set; }


        public CityInfo()
        {
            
        }
        public string GetProvince()
        {
            return this.Province;
        }

        public float GetPopulation()
        {
            return this.Population;
        }

        public string GetLocation()
        {
            return "Latitude "+ this.Lat + "Longitude: " + this.Lng;
        }
    }
}
