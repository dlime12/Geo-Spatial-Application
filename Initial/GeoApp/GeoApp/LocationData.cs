using System;
using System.Collections.Generic;
namespace GeoApp
{
    public class LocationData
    {
        public string Name{ get; set; }
        public float FloatMeta{ get; set; }
        public string StringMeta { get; set; }
        public int IntMeta{ get; set; }

        public double Accuracy{ get; set; }
        public double Longitude{ get; set; }
        public double Latitude{ get; set; }
        public double Flatitude{ get; set; }

        public LocationData(){

        }

        public LocationData(string name, float floatMeta, string stringMeta, int intMeta, 
                            double accuracy, double longitude, double latitude, double flatitude)
        {
            Name = name;
            FloatMeta = floatMeta;
            StringMeta = stringMeta;
            IntMeta = intMeta;

            Accuracy = accuracy;
            Longitude = longitude;
            Latitude = latitude;
            Flatitude = flatitude;
        }

        public string ToCSVString(){
            string csvstring;

            List <string> locationDetails = new List<string>();
            // add location details into the list

            locationDetails.Add(Name);
            locationDetails.Add(FloatMeta.ToString());
            locationDetails.Add(StringMeta);
            locationDetails.Add(IntMeta.ToString());
            locationDetails.Add(Accuracy.ToString());
            locationDetails.Add(Longitude.ToString());
            locationDetails.Add(Latitude.ToString());
            locationDetails.Add(Flatitude.ToString());

            csvstring = String.Join(",", locationDetails); // join strings in list with ',' seperator for csv

            return csvstring;
        }
    }
}
