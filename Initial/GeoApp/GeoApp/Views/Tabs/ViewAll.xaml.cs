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
using System.Collections.ObjectModel;

using GeoApp.Views;


namespace GeoApp.Views.Tabs
{
    public partial class ViewAll : ContentPage
    {
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");
        public ViewAll()
        {
            InitializeComponent();
        }

        async private void RefreshClicked(Object sneder, EventArgs args)
        {
            string json = System.IO.File.ReadAllText(fileName);

            // turn data into Feature collection
            FeatureCollection result = JsonConvert.DeserializeObject<FeatureCollection>(json);

            foreach (Feature feature in result.Features)
            {
                //PointView.ItemSource = feature.Properties;
                //await DisplayAlert("Data Test", feature.Properties, "OK");
            }
       
            
                
            
        }


    }
}
