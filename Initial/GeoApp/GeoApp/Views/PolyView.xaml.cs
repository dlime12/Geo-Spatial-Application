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
using Point = GeoJSON.Net.Geometry.Point;


namespace GeoApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PolyView : ContentPage
	{
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");

        public int num_pos = 0;

        List<Position> positions = new List<Position>();
        public List<Xamarin.Forms.GoogleMaps.Position> googPositions = new List<Xamarin.Forms.GoogleMaps.Position>();

        public PolyView ()
		{
			InitializeComponent ();
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

        private void AddPointButtonClicked(object sender, EventArgs args){
            // add coordinates
            Position position = new Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text),
                                              double.Parse(flatitudeLabel.Text));
            Xamarin.Forms.GoogleMaps.Position positionGoog = new Xamarin.Forms.GoogleMaps.Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text));


            positions.Add(position); // add to positions list
            googPositions.Add(positionGoog);
            num_pos++;

            pointLabel1.Text = "Points added: " + num_pos;

            pointLabel1.IsVisible = true;

            Reset_Location();

        }


        private void CloseButtonClicked(object sender, EventArgs args){

            positions.Add(positions[0]); // add same coordinate as first position

            closeLabel.IsVisible = true;

            AddButton.IsVisible = true;

            Reset_Location();
        }

        // when add buttton is pressed create a new lcoation feature  
        async void AddButtonClicked(object sender, EventArgs args)
        {
            // Check if name is empty or not, if yes then terminate with error message
            if (string.IsNullOrEmpty(nameEntered.Text))
            {
                await DisplayAlert("Status", "Polygon Name is Required!", "OK");
            }

            else
            {
                AddButton.IsEnabled = false;

                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                AddButton.IsEnabled = false;
                string file = System.IO.File.ReadAllText(fileName);

                List<LineString> lineString = new List<LineString>();

                LineString line = new LineString(positions);
                lineString.Add(line);

                Polygon polygon1 = new Polygon(lineString);

                Feature feature1 = new Feature(polygon1); // create new feature 

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

                string updatedJson1 = AddObjectsToJson(file, json1); // add feature to feature collection 

                System.IO.File.WriteAllText(fileName, updatedJson1); // update saved json file 

                await DisplayAlert("Status", "Polygon data added.", "OK");

                await Navigation.PopModalAsync();
            }

            /*
            Polygon polygon = new Polygon(lineString);

            Feature feature = new Feature(polygon); // create new feature 

            feature.Properties.Add("Name", nameEntered.Text);
            feature.Properties.Add("FloatMeta", float.Parse(floatEntered.Text));
            feature.Properties.Add("StringMeta", stringEntered.Text);
            feature.Properties.Add("IntMeta", int.Parse(intEntered.Text));

            string json = JsonConvert.SerializeObject(feature);

            string updatedJson = AddObjectsToJson(file, json); // add feature to feature collection 

            System.IO.File.WriteAllText(fileName, updatedJson); // update saved json file 
            */

            Xamarin.Forms.GoogleMaps.Polygon polyObject = new Xamarin.Forms.GoogleMaps.Polygon();
            foreach (Xamarin.Forms.GoogleMaps.Position pos in googPositions)
            {
                polyObject.Positions.Add(pos);
            }
            polyObject.StrokeWidth = 2f;
            polyObject.StrokeColor = Color.Black;
            polyObject.FillColor = Color.FromRgba(0, 52, 70, 70);
            polyObject.Tag = nameEntered.Text;
            polyObject.IsClickable = true;
            ViewMain.plotPolygon plotLine = new ViewMain.plotPolygon(polyObject, float.Parse(floatEntered.Text), stringEntered.Text, int.Parse(intEntered.Text));

            await DisplayAlert("Status", "Polygon data added.", "OK");

            await Navigation.PopModalAsync();
        }

        // method to add feature 
        public string AddObjectsToJson(string json, string objects)
        {
            FeatureCollection list = JsonConvert.DeserializeObject<FeatureCollection>(json);
            Feature feature = JsonConvert.DeserializeObject<Feature>(objects);

            list.Features.Add(feature);

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }


        private void Reset_Location()
        {
            accuracyLabel.Text = null;
            longitudeLabel.Text = null;
            latitudeLabel.Text = null;
            flatitudeLabel.Text = null;
        }
    }
}