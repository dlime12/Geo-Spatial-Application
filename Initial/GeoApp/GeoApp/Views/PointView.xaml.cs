using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;
using Xamarin.Forms.GoogleMaps;
using Point = GeoJSON.Net.Geometry.Point;


namespace GeoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointView : ContentPage
    {
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");

        public PointView()
        {
            InitializeComponent();

        }

        async void WatcherButtonClicked(object sender, EventArgs args)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            accuracyLabel.Text = location.Accuracy.ToString(); // show accuracy 
        }

        async void SetButtonClicked(object sender, EventArgs args)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            accuracyLabel.Text = location.Accuracy.ToString();
            longitudeLabel.Text = location.Longitude.ToString();
            latitudeLabel.Text = location.Latitude.ToString();
            flatitudeLabel.Text = location.Altitude.ToString();
        }

        // when add buttton is pressed create a new lcoation feature  
        async void AddButtonClicked(object sender, EventArgs args)
        {
            AddButton.IsEnabled = false;
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            string file = System.IO.File.ReadAllText(fileName);

            GeoJSON.Net.Geometry.Position geoPosition = new GeoJSON.Net.Geometry.Position(location.Latitude, location.Longitude, location.Altitude);
            Xamarin.Forms.GoogleMaps.Position googPosition = new Xamarin.Forms.GoogleMaps.Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text));

            Point point = new Point(geoPosition); // add coordinates

            Feature feature = new Feature(point); // create new feature 

            /*

            feature.Properties.Add("Name", nameEntered.Text);
            feature.Properties.Add("FloatMeta", float.Parse(floatEntered.Text));
            feature.Properties.Add("StringMeta", stringEntered.Text);
            feature.Properties.Add("IntMeta", int.Parse(intEntered.Text));

            string json = JsonConvert.SerializeObject(feature);

            string updatedJson = AddObjectsToJson(file, json); // add feature to feature collection 

            System.IO.File.WriteAllText(fileName, updatedJson); // update saved json file
            */

            Pin pin = new Pin();
            pin.Position = googPosition;
            pin.Type = PinType.Place;
            pin.Label = nameEntered.Text;
            ViewMain.plotPin plotPoint = new ViewMain.plotPin(pin, float.Parse(floatEntered.Text), stringEntered.Text, int.Parse(intEntered.Text));


            await DisplayAlert("Status", "Point data added.", "OK");


            await Navigation.PopModalAsync();
            // Check if name is empty or not, if yes then terminate with error message
            if (string.IsNullOrEmpty(nameEntered.Text))
            {
                await DisplayAlert("Status", "Point Name is Required!", "OK");
            }
            else
            {
                AddButton.IsEnabled = false;
                var request1 = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location1 = await Geolocation.GetLocationAsync(request1);

                string file1 = System.IO.File.ReadAllText(fileName);

                GeoJSON.Net.Geometry.Position position = new GeoJSON.Net.Geometry.Position(location1.Latitude, location1.Longitude, location1.Altitude);

                Point point1 = new Point(position); // add coordinates
                
                Feature feature1 = new Feature(point1); // create new feature 

                feature1.Properties.Add("Name", nameEntered.Text);
                if (string.IsNullOrEmpty(floatEntered.Text))
                {
                    feature1.Properties.Add("FloatMeta", 0.0);
                }
                else
                {
                    feature1.Properties.Add("FloatMeta", float.Parse(floatEntered.Text));
                }
                if (string.IsNullOrEmpty(stringEntered.Text))
                {
                    feature1.Properties.Add("StringMeta", "null");
                }
                else
                {
                    feature1.Properties.Add("StringMeta", stringEntered.Text);
                }
                if (string.IsNullOrEmpty(floatEntered.Text))
                {
                    feature1.Properties.Add("IntMeta", 0);
                }
                else
                {
                    feature1.Properties.Add("IntMeta", int.Parse(intEntered.Text));
                }

                string json1 = JsonConvert.SerializeObject(feature1);

                string updatedJson1 = AddObjectsToJson(file1, json1); // add feature to feature collection 

                System.IO.File.WriteAllText(fileName, updatedJson1); // update saved json file 


                await DisplayAlert("Status", "Point data added.", "OK");


                await Navigation.PopModalAsync();
            }
        }

        // method to add feature 
        public string AddObjectsToJson(string json, string objects)
        {
            FeatureCollection list = JsonConvert.DeserializeObject<FeatureCollection>(json);
            Feature feature = JsonConvert.DeserializeObject<Feature>(objects);

            list.Features.Add(feature);

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }


    }
}