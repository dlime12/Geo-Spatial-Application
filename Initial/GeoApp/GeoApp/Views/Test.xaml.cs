using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Plugin.Messaging;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using PCLStorage;

namespace GeoApp.Views
{
    public partial class Test : ContentPage
    {
        public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");
        

        public Test()
        {
            InitializeComponent();
        }

        // show point data when clicked 
        private void LoadButtonClicked(object sender, EventArgs args)
        {
            string json = System.IO.File.ReadAllText(fileName);

            // turn data into Feature collection
            FeatureCollection result = JsonConvert.DeserializeObject<FeatureCollection>(json);

            foreach (Feature feature in result.Features)
            {

                pointView.ItemsSource = feature.Properties;// set source as properties list
            }

        }

        private void GeoButtonClicked(Object sneder, EventArgs args)
        {
            string json = System.IO.File.ReadAllText(fileName);
            DisplayAlert("GeoJson File", json, "OK");
        }

        private void SendButtonClicked(Object sender, EventArgs args)
        {
            string json = File.ReadAllText(fileName);

            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                // Alternatively use EmailBuilder fluent interface to construct more complex e-mail with multiple recipients, bcc, attachments etc.
                var email = new EmailMessageBuilder()
                  .To("jadekelley25@gmail.com")
                  .Subject("GeoJson Data ")
                    .Body(json)
                  .Build();

                emailMessenger.SendEmail(email);
            } else {
                DisplayAlert("Error", "Can't send with attachment", "OK");
            }
        }
    }
}
