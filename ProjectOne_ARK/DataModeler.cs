/*
 * Program:         Data parser        
 * Module:          DataModeler.cs
 *                  Ruben Dario  Mejia Cardona
 * Date:            February 17, 2022
 * Description:     Non-generic class that will parse data files XML, JSON, CSV.
 */


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Microsoft.VisualBasic.FileIO;



namespace ProjectOne_ARK
{
    // Helper Class that Hold Canadian cities elements
    public class DataModeler
    {
        CityInfo canadianCitiesInfo = new CityInfo();

        // Properties
        private XmlDocument doc;
        private TextFieldParser parser;
        public List<CityInfo> infoCityList;
        private Dictionary<string, List<CityInfo>> dataModelerDict = new Dictionary<string, List<CityInfo>>();
        public List<CityInfo> infoCities;


        // Methods

        // Customized delegate matching the signature of the three parsing methods
        public delegate void MyDelagate(string file);

        // Attempts to read the file specified by 'path' into the string
        // Returns 'true' if successful or 'false' if it fails
        public bool ReadFile(string path, out string data)
        {
            try
            {
                // Read JSON file data 
                data = File.ReadAllText(path);
                return true;
            }
            catch
            {
                data = null;
                return false;
            }
        }

        // Parsing method for XML
        public void ParseXML(string xmlFile)
        {
            // Attempts to read the XML file

            try
            {
                doc.Load(xmlFile);
            }
            catch (Exception)
            {
                Console.WriteLine("\nERROR: Failed to convert data in XML file to an DataModeler object");
            }
        }

        // Parsing method for JSON
        public void ParseJSON(string jsonFile)
        {
            // Attempts to read the JSON file, returns 'true' if successful or 'false' if it fails
            string jsonData;
            if (ReadFile(jsonFile, out jsonData))
            {
                try
                {
                    // Read JSON file and convert to an DataModeler object
                    infoCityList = JsonConvert.DeserializeObject<List<CityInfo>>(jsonData);
                }
                catch (JsonException)
                {
                    Console.WriteLine("\nERROR: Failed to convert data in JSON file to an DataModeler object");
                }
            }
            else
                // Read operation for data failed
                Console.WriteLine("\nERROR:\tUnable to read the data file. Try another path.");
        }

        // Parsing method for CSV
        public void ParseCSV(string csvFile)
        {
            try
            {
                parser = new TextFieldParser(csvFile);
            }
            catch (Exception)
            {
                Console.WriteLine("\nERROR:\tUnable to read the data file. Try another path.");
            }
        }

        // Invoke one of the parsing methods ParseXML, ParseJSON, ParseCSV and 
        // return the value of the generic type dictionary
        public Dictionary<string, List<CityInfo>> ParseFile(string fileName, string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    MyDelagate xmlDel = ParseXML;

                    doc = new XmlDocument();
                    xmlDel.Invoke(fileName);
                    XPathNavigator nav = doc.CreateNavigator();

                    int idx = 1;

                    string query = $"//CanadaCity";
                    XPathNodeIterator nodeIt = nav.Select(query);

                    while (nodeIt.MoveNext())
                    {
                        XPathExpression ct = XPathExpression.Compile($"string(//CanadaCity[{idx}]//city)");
                        XPathExpression ca = XPathExpression.Compile($"string(//CanadaCity[{idx}]//city_ascii)");
                        XPathExpression lt = XPathExpression.Compile($"string(//CanadaCity[{idx}]//lat)");
                        XPathExpression lg = XPathExpression.Compile($"string(//CanadaCity[{idx}]//lng)");
                        XPathExpression co = XPathExpression.Compile($"string(//CanadaCity[{idx}]//country)");
                        XPathExpression an = XPathExpression.Compile($"string(//CanadaCity[{idx}]//admin_name)");
                        XPathExpression cp = XPathExpression.Compile($"string(//CanadaCity[{idx}]//capital)");
                        XPathExpression pt = XPathExpression.Compile($"string(//CanadaCity[{idx}]//population)");
                        XPathExpression id = XPathExpression.Compile($"string(//CanadaCity[{idx}]//id)");

                        infoCities = new List<CityInfo>()
                    {
                        new CityInfo
                        {
                            City = nav.Evaluate(ct).ToString(),
                            City_Ascii = nav.Evaluate(ca).ToString(),
                            Lat = Convert.ToDouble(nav.Evaluate(lt)),
                            Lng = Convert.ToDouble(nav.Evaluate(lg)),
                            Country = nav.Evaluate(co).ToString(),
                            Admin_Name = nav.Evaluate(an).ToString(),
                            Capital = nav.Evaluate(cp).ToString(),
                            Population = Convert.ToInt32(nav.Evaluate(pt))
                        }
                    };
                        dataModelerDict.Add(nav.Evaluate(id).ToString(), infoCities);

                        idx++;
                    }

                    return dataModelerDict;

                case "json":
                    MyDelagate jsonDel = ParseJSON;
                    infoCityList = new List<CityInfo>();
                    jsonDel.Invoke(fileName);

                    foreach (var item in infoCityList)
                    {
                        infoCities = new List<CityInfo>()
                            {
                                new CityInfo
                                {
                                    City = item.City,
                                    City_Ascii = item.City_Ascii,
                                    Lat = item.Lat,
                                    Lng = item.Lng,
                                    Country = item.Country,
                                    Admin_Name = item.Admin_Name,
                                    Capital = item.Capital,
                                    Population = item.Population
                                }
                            };

                        dataModelerDict.Add(item.Id.ToString(), infoCities);
                        dataModelerDict.Remove("0");
                    };

                    return dataModelerDict;
                case "csv":
                    MyDelagate csvDel = ParseCSV;
                    csvDel.Invoke(fileName);

                    parser.HasFieldsEnclosedInQuotes = false;
                    parser.SetDelimiters(",");
                    parser.Delimiters = new string[] { "," };
                    while (true)
                    {
                        string[] parts = parser.ReadFields();
                        if (parts == null)
                            break;
                        else if (parts[0] != "city")
                        {
                            infoCities = new List<CityInfo>()
                        {
                            new CityInfo
                            {
                                City = parts[0],
                                City_Ascii = parts[1],
                                Lat = Convert.ToDouble(parts[2]),
                                Lng = Convert.ToDouble(parts[3]),
                                Country = parts[4],
                               Admin_Name= parts[5],
                                Capital = parts[6],
                                Population = Convert.ToInt32(parts[7])
                            }
                        };
                            dataModelerDict.Add(parts[8].ToString(), infoCities);
                        }
                    }
                    parser.Close();

                    return dataModelerDict;
                default:
                    break;
            } // end switch
            return null;
        }
    }
}