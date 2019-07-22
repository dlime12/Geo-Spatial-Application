using System;

using Xamarin.Forms;

namespace GeoApp.Views.Tabs
{
    public class ViewLine : ContentPage
    {
        public ViewLine()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

