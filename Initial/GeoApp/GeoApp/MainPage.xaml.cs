using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GeoApp.Views;
using System.IO;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using GeoApp.Views.Tabs;

namespace GeoApp
{
    public partial class MainPage : MasterDetailPage
    {
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");

        public MainPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new ViewMain());
            IsPresented = false;



            // initialise start of geojson file 
            FeatureCollection featureCollection = new FeatureCollection();

            string json = JsonConvert.SerializeObject(featureCollection, Formatting.Indented);

            System.IO.File.WriteAllText(fileName, json); // save to file 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ViewMain());

            IsPresented = false;
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new CollectedData());
            IsPresented = false;
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ImpExport());
            IsPresented = false;

        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Settings());
            IsPresented = false;
        }


    }
}
