using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SmartHouseControlPanel
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SmartUpDB;TrustServerCertificate=True";
        private DispatcherTimer timer;
        private string videoPath = @"C:\\Users\\armen\\Downloads\\Video Camera Recording Frame Black 1.mp4\";
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            _ = DisplayCurrentTemperature();
           
        }
        private async Task<double> DisplayCurrentTemperature()
        {
            try
            {
                string apiKey = "d098f796ac46f79e804a0d738750237c";
                string city = "Yerevan";
                string country = "AM";

                string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},{country}&units=metric&appid={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                        if (data != null && data.main != null && data.main.temp != null)
                        {
                            double airTemp = data.main.temp;
                            txtOutsideTemperature.Text = $"Outside Temperature: {airTemp.ToString("0.0")}°C";
                            RecommendTemperature(airTemp);
                            return airTemp;
                        }
                        else
                        {
                            MessageBox.Show("Failed to parse weather data.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to fetch weather data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return double.NaN;
        }


        private void RecommendTemperature(double outsideTemperature)
        {
            double recommendedTemperature;

            if (outsideTemperature < 10)
            {
                recommendedTemperature = 25; 
            }
            else if (outsideTemperature >= 10 && outsideTemperature < 20)
            {
                recommendedTemperature = 22; 
            }
            else if (outsideTemperature >= 20 && outsideTemperature < 30)
            {
                recommendedTemperature = 20; 
            }
            else
            {
                recommendedTemperature = 18; 
            }
            recTemperature.Text = $"Recommended Temperature: {recommendedTemperature}°C";
        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        private void TurnOnCameras_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri(videoPath);
            mediaElement.Play();
            UpdateDeviceStatus("Cameras", 1);
        }

        private void SubmitTemperatures_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtWaterTemp.Text, out int waterTemp))
            {
                InsertTemperature("Water", waterTemp);
            }
            else
            {
                MessageBox.Show("Please enter a valid water temperature.");
            }
            if (int.TryParse(txtAirTemp.Text, out int airTemp))
            {
                InsertTemperature("Air", airTemp);
            }
            else
            {
                MessageBox.Show("Please enter a valid air temperature.");
            }
            if (int.TryParse(txtSaunaTemp.Text, out int saunaTemp))
            {
                InsertTemperature("Sauna", saunaTemp);
            }
            else
            {
                MessageBox.Show("Please enter a valid sauna temperature.");
            }
        }

        private void InsertTemperature(string sensor, int temperature)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO TemperatureData (Sensor, Temperature, Timestamp) VALUES (@Sensor, @Temperature, GETDATE())";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Sensor", sensor);
                    command.Parameters.AddWithValue("@Temperature", temperature);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Temperature data inserted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void OpenGate_Click(object sender, RoutedEventArgs e)
        {
            UpdateDeviceStatus("Gate", 1);
        }

        private void TurnOnCoffeeMachine_Click(object sender, RoutedEventArgs e)
        {
            UpdateDeviceStatus("CoffeeMachine", 1);
        }

        private void TurnOnLights_Click(object sender, RoutedEventArgs e)
        {
            UpdateDeviceStatus("Lights", 1);
        }

        private void UpdateDeviceStatus(string device, int status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO DeviceStatus (Device, Status, Timestamp) VALUES (@Device, @Status, GETDATE())";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Device", device);
                    command.Parameters.AddWithValue("@Status", status);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show($"{device} status updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CheckForDamage()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Random random = new Random();
                    int damageIndicator = random.Next(2);
                    string query = "INSERT INTO DamageTable (DamageIndicator, Timestamp) VALUES (@DamageIndicator, GETDATE())";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DamageIndicator", damageIndicator);
                    command.ExecuteNonQuery();
                    if (damageIndicator == 1)
                    {
                        MessageBox.Show("Damage has been detected in your house!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}


