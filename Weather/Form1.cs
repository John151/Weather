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
using System.IO;

namespace Weather
{
    public partial class Form1 : Form
    {
        readonly string BaseUrl = "https://weather-csharp.herokuapp.com/";

        string[] States = {"Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado",
            "Connecticut", "Delaware", "District of Columbia", "Florida", "Georgia", "Hawaii",
            "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine",
            "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri",
            "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York",
            "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania",
            "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah",
            "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming" };


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
            string state = cbxState.Text;

            //fetch current weather and display

            if (LocationDataValid(city, state))
            {

            
                if (GetWeatherText(city, state, out string
                    weather, out string textErrorMessage))
                {
                    lblWeather.Text = weather;
                }
            else
                {
                    MessageBox.Show(textErrorMessage, "Error");
                }

                if (picWeather.Image != null)
                {
                    picWeather.Image.Dispose(); //clear previous image
                }
                if (GetWeatherImage(city, state, out Image image, out string imageErrorMessage))
                {
                    picWeather.Image = image;
                }
            }
            else
            {
                MessageBox.Show("Enter both city and state", "Error");
            }

            btnGetWeather.Enabled = true;

        }

        private bool GetWeatherImage(string city, string state,
            out Image weatherImage, out string errorMessage)
        {
            weatherImage = null; //initialize the out parameters
            errorMessage = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    //use the format methof to make a string in the format
                    // http://weater-csharp.herokuapp.com/text?city=dallas&state=tx
                    string weatherPhotoUrl = String.Format("{0}photo?city={1}&state={2}",
                        BaseUrl, city, state);
                    string tempFileDirectory = Path.GetTempPath().ToString(); //directory to save the image
                    string weatherFilePath = Path.Combine(tempFileDirectory, "weather_image.jpeg");
                    Debug.WriteLine(weatherFilePath);
                    client.DownloadFile(weatherPhotoUrl, weatherFilePath);
                    weatherImage = Image.FromFile(weatherFilePath);
                }
                return true; //request made, no errors
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }
        }
        private bool LocationDataValid(string city, string state)
        {
            //check if data is entered
            if (String.IsNullOrWhiteSpace(city))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(state))
            {
                return false;
            }
            //if all checks passed, return true
            return true;
        }
        private bool GetWeatherText(string city, string state, out string weatherText, out string errorMessage)
        {
            //use the format methof to make a string in the format
            // http://weater-csharp.herokuapp.com/text?city=dallas&state=tx

            string weatherTextUrl = String.Format("{0}text?city={1})&state={2}", BaseUrl, city, state);
            Debug.WriteLine(weatherTextUrl); //message for developer

            errorMessage = null;
            weatherText = null;

            try
            {

                using (WebClient client = new WebClient())
                {
                    weatherText = client.DownloadString(weatherTextUrl);
                }

                Debug.WriteLine(weatherText);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbxState.Items.AddRange(States);
        }
    }
}
