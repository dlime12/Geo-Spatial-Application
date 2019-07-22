using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Plugin.FilePicker;
using Xamarin.Forms.PlatformConfiguration;
using Plugin.FilePicker.Abstractions;


//using Newtonsoft.Json;

namespace GeoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImpExport : ContentPage
    {
        // CSV Attachment
        Attachment JSON_attachment;

        string email_Address;

        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.json");


        public ImpExport()
        {
            InitializeComponent();
        }


        private void GeoButtonClicked(Object sneder, EventArgs args)
        {
            string json = System.IO.File.ReadAllText(fileName);
            DisplayAlert("GeoJson File", json, "OK");
        }

        async private void SendButtonClicked(Object sender, EventArgs args)
        {
            try
            {
                string json = File.ReadAllText(fileName);
                JSON_attachment = new Attachment(fileName);

                email_Address = emailEntered.Text;


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("GeoApp@gmail.com");

                mail.To.Add(email_Address);

                mail.Subject = "Your requested Geo Application CSV dataset";
                mail.Body = "The following attachment is your request dataset contained into a GEO JSON format";


                // Attaching attachment files into mails
                mail.Attachments.Add(JSON_attachment);


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("geoapplicationqut@gmail.com", "Geoapplication123");
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                SmtpServer.Send(mail);

                await DisplayAlert("Status", "Email sent successfully", "OK");
            }

            catch (Exception mailNotSent)
            {
                await DisplayAlert("Status", "Unable to send email. Error :" + mailNotSent, "OK");
            }

        }


        async private void ImportButtonClicked(Object sender, EventArgs args)
        {


            var answer = await DisplayAlert("Are you Sure?", "Importing will overwrite the current data", "No", "Yes");
            
            if (!answer)
            {
                try
                {

                    FileData JSON_filePath = await CrossFilePicker.Current.PickFile();
                    if (JSON_filePath != null)
                    {
                        string json3 = JSON_filePath.FilePath;
 

                        string json2 = System.IO.File.ReadAllText(json3);
                      
                        await DisplayAlert("GeoJson File", json2, "OK");
                        File.WriteAllText(fileName, json2);


                        await DisplayAlert("Status", "JSON file imported successfully", "OK");

                    }

                    else
                    {
                        await DisplayAlert("Status", "File Path doesn't exist or is invalid", "OK");
                    }

                }

                catch (Exception importError)
                {
                    await DisplayAlert("Status", "Unable to import JSON file. Error :" + importError, "OK");
                }
            }
        }

    }
}