using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace GeoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMain : ContentPage
    {
        public static Map map = new Map();
        public ViewMain()
        {
            InitializeComponent();
            displayMap(map);

            var settings = new ToolbarItem
            {
                Text = "Add",
                Command = new Command(this.ShowAddPopup),
            };
            this.ToolbarItems.Add(settings);
        }

        async void ShowAddPopup()
        {
            var action = await DisplayActionSheet("Add new data? ", "Cancel", null, "Point", "Line", "Polygon");
            switch (action)
            {
                case "Point":
                    await Navigation.PushModalAsync(new PointView());
                    break;
                case "Line":
                    await Navigation.PushModalAsync(new LineView());
                    break;
                case "Polygon":
                    await Navigation.PushModalAsync(new PolyView());
                    break;
            }
        }



        public void displayMap(Map map)
        {
            MapSpan.FromCenterAndRadius(new Position(-27.477176, 153.028439), Distance.FromKilometers(0.3));
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(-27.477176, 153.028439), 15);


            map.HeightRequest = 100;
            map.WidthRequest = 100;
            map.VerticalOptions = LayoutOptions.FillAndExpand;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }

        public class plotPin
        {

            public plotPin(Pin pin, float floatMeta, string stringMeta, int intMeta)
            {

                map.PinClicked += (sender, e) =>
                {
                    //e.Pin.Label = null;
                    map.SelectedPin = null;
                    point_clicked(sender, e, pin, floatMeta, stringMeta, intMeta);
                };
                map.Pins.Add(pin);
            }

            public void point_clicked(object sender, EventArgs e, Pin pin, float floatMeta, string stringMeta, int intMeta)
            {
                Application.Current.MainPage.DisplayAlert(pin.Label ,string.Format("Float: {0}\nString: {1}\nInt: {2}", floatMeta, stringMeta, intMeta) , "cancel");
            }

        }

        public class plotLine
        {
            public plotLine(Polyline line, float floatMeta, string stringMeta, int intMeta)
            {
                line.Clicked += (sender, e) => line_clicked(sender, e, line, floatMeta, stringMeta, intMeta);
                map.Polylines.Add(line);
            }

            public void line_clicked(object sender, EventArgs e, Polyline line, float floatMeta, string stringMeta, int intMeta)
            {
                Application.Current.MainPage.DisplayAlert(line.Tag.ToString(), string.Format("Float: {0}\nString: {1}\nInt: {2}", floatMeta, stringMeta, intMeta), "cancel");
            }
        }

        public class plotPolygon
        {
            public plotPolygon(Polygon polygon, float floatMeta, string stringMeta, int intMeta)
            {
                polygon.Clicked += (sender, e) => polygon_clicked(sender, e, polygon, floatMeta, stringMeta, intMeta);
                map.Polygons.Add(polygon);
            }

            public void polygon_clicked(object sender, EventArgs e, Polygon polygon, float floatMeta, string stringMeta, int intMeta)
            {
                Application.Current.MainPage.DisplayAlert(polygon.Tag.ToString(), string.Format("Float: {0}\nString: {1}\nInt: {2}", floatMeta, stringMeta, intMeta), "cancel");
            }
        }
    }
}