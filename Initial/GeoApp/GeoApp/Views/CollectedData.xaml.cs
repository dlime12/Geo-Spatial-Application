using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoApp.Views.Tabs;

namespace GeoApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectedData : TabbedPage
	{
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");

        public CollectedData ()
		{
			InitializeComponent ();

            Children.Add(new ViewAll());
            Children.Add(new ViewPoint());
            Children.Add(new ViewLine());
            Children.Add(new ViewPolygon());

        }

        //// show point data when clicked 
        //private void LoadButtonClicked(object sender, EventArgs args)
        //{
        //    string json = System.IO.File.ReadAllText(fileName);

        //    // turn data into Feature collection
        //    FeatureCollection result = JsonConvert.DeserializeObject<FeatureCollection>(json);

        //    foreach (Feature feature in result.Features)
        //    {

        //       /// pointView.ItemsSource = feature.Properties.Values;// set source as name

        //    }

        //}

        //private void GeoButtonClicked(Object sneder, EventArgs args)
        //{
        //    string json = System.IO.File.ReadAllText(fileName);
        //    DisplayAlert("GeoJson File", json, "OK");
        //}
    }
}