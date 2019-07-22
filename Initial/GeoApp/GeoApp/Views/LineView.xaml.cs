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
using Xamarin.Forms.GoogleMaps;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;
using Position = GeoJSON.Net.Geometry.Position;

namespace GeoApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LineView : ContentPage
	{
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");

        public List<Position> positions = new List<Position>();
        public List<Xamarin.Forms.GoogleMaps.Position> googPositions = new List<Xamarin.Forms.GoogleMaps.Position>();

        public LineView ()
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

        private void AddStartLineButtonClicked(object sender, EventArgs args){
            // add coordinates
            Position position1 = new Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text),
                                              double.Parse(flatitudeLabel.Text));
            Xamarin.Forms.GoogleMaps.Position position1Goog = new Xamarin.Forms.GoogleMaps.Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text));
                                              
            positions.Add(position1); // add to positions list
            googPositions.Add(position1Goog);
            pointLabel1.IsVisible = true;

            Reset_Location(); // empty gps data
        }

        private void AddEndLineButtonClicked(object sender, EventArgs args)
        {
            // add coordinates
            Position position2 = new Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text),
                                              double.Parse(flatitudeLabel.Text));
            Xamarin.Forms.GoogleMaps.Position position2Goog = new Xamarin.Forms.GoogleMaps.Position(double.Parse(latitudeLabel.Text), double.Parse(longitudeLabel.Text));

            positions.Add(position2); // add to positions list
            googPositions.Add(position2Goog);
            pointLabel2.IsVisible = true;

            Reset_Location(); // empty gps data

        }


        // when add buttton is pressed create a new lcoation feature  
        async void AddButtonClicked(object sender, EventArgs args)
        {
            string file = System.IO.File.ReadAllText(fileName);
            // add coordinates  

            LineString lineString = new LineString(positions); // create linestring using positions 

            Feature feature = new Feature(lineString); 
            /*
            // add propertires 
            feature.Properties.Add("Name", nameEntered.Text);
            feature.Properties.Add("FloatMeta", float.Parse(floatEntered.Text));
            feature.Properties.Add("StringMeta", stringEntered.Text);
            feature.Properties.Add("IntMeta", int.Parse(intEntered.Text));

            string json = JsonConvert.SerializeObject(feature);

            string updatedJson = AddObjectsToJson(file, json); // add feature to feture collection

            System.IO.File.WriteAllText(fileName, updatedJson);
            */

            Polyline line = new Polyline();
            foreach(Xamarin.Forms.GoogleMaps.Position pos in googPositions)
            {
                line.Positions.Add(pos);
            }
            line.StrokeColor = Color.Red;
            line.StrokeWidth = 5f;
            line.Tag = nameEntered.Text;
            line.IsClickable = true;
            ViewMain.plotLine plotLine = new ViewMain.plotLine(line, float.Parse(floatEntered.Text), stringEntered.Text, int.Parse(intEntered.Text));

            await DisplayAlert("Status", "Data Added", "OK");

            await Navigation.PopModalAsync();
            // Check if name is empty or not, if yes then terminate with error message
            if (string.IsNullOrEmpty(nameEntered.Text))
            {
                await DisplayAlert("Status", "Line Name is Required!", "OK");
            }

            else
            {
                string file1 = System.IO.File.ReadAllText(fileName);
                // add coordinates  

                LineString lineString1 = new LineString(positions); // create linestring using positions 

                Feature feature1 = new Feature(lineString1);

                // add propertires 
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

                string updatedJson1 = AddObjectsToJson(file1, json1); // add feature to feture collection

                System.IO.File.WriteAllText(fileName, updatedJson1);

                await DisplayAlert("Status", "Data Added", "OK");

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

        private void Reset_Location(){
            accuracyLabel.Text = null;
            longitudeLabel.Text = null;
            latitudeLabel.Text = null;
            flatitudeLabel.Text = null;
        }

    }
}