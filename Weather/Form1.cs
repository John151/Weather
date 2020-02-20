using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace Weather
{
    public partial class Form1 : Form
    {
        readonly string BaseUrl = "https://weather-csharp.herokuapp.com/";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetWeather_Click(object sender, EventArgs e)
        {
            //disable button to prevent multiple requests before one is complete
            btnGetWeather.Enabled = false;

            //read the data from the TextBoxes
            string city = txtCity.Text;
            string state = txtState.Text;

            //fetch current weather and display
            string weather = GetWeatherText(city, state);
            lblWeather.Text = weather;

            //re-enable button for another request
            btnGetWeather.Enabled = true;
        }

        private string GetWeatherText(string city, string state)
        {
            //use the format methof to make a string in the format
            // http://weater-csharp.herokuapp.com/text?city=dallas&state=tx

            string weatherTextUrl = String.Format("{0}text?city={1})&state={2}", BaseUrl, city, state);
            Debug.WriteLine(weatherTextUrl); //message for developer
            string weatherText; //left off here!! pick up here!!

            using (WebClient client = new WebClient())
            {
                weatherText = client.DownloadString(weatherTextUrl);
            }

            return weatherText;
        }

    }
}
