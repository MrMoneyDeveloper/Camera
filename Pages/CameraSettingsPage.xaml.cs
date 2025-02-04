using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Camera.Models; // Import CameraSettings model

namespace Camera.Pages
{
    public partial class CameraSettingsPage : ContentPage
    {
        private List<CameraSettings> allSettings = new(); // Ensure initialization

        public CameraSettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Loads the latest saved camera settings from file and updates UI.
        /// </summary>
        private void LoadSettings()
        {
            allSettings = CameraSettings.LoadAllSettings();

            var latestSettings = allSettings.OrderByDescending(s => s.Id).FirstOrDefault() ?? new CameraSettings();

            // Populate UI fields safely
            Camera1IP.Text = latestSettings.Camera1_IP ?? "";
            Camera1Port.Text = latestSettings.Camera1_Port ?? "";
            Camera1Username.Text = latestSettings.Camera1_Username ?? "";
            Camera1Password.Text = latestSettings.Camera1_Password ?? "";

            Camera2IP.Text = latestSettings.Camera2_IP ?? "";
            Camera2Port.Text = latestSettings.Camera2_Port ?? "";
            Camera2Username.Text = latestSettings.Camera2_Username ?? "";
            Camera2Password.Text = latestSettings.Camera2_Password ?? "";

            Camera3IP.Text = latestSettings.Camera3_IP ?? "";
            Camera3Port.Text = latestSettings.Camera3_Port ?? "";
            Camera3Username.Text = latestSettings.Camera3_Username ?? "";
            Camera3Password.Text = latestSettings.Camera3_Password ?? "";

            Camera4IP.Text = latestSettings.Camera4_IP ?? "";
            Camera4Port.Text = latestSettings.Camera4_Port ?? "";
            Camera4Username.Text = latestSettings.Camera4_Username ?? "";
            Camera4Password.Text = latestSettings.Camera4_Password ?? "";
        }

        /// <summary>
        /// Validates only the filled camera fields before saving.
        /// </summary>
        private bool ValidateInputs()
        {
            bool isValid = true;
            List<string> errorMessages = new();

            // Function to validate a single camera, but only if it's filled
            void ValidateCamera(string ip, string port, string username, int cameraNumber)
            {
                if (string.IsNullOrWhiteSpace(ip) && string.IsNullOrWhiteSpace(port) && string.IsNullOrWhiteSpace(username))
                {
                    return; // Ignore empty cameras
                }

                if (string.IsNullOrWhiteSpace(ip) || !IsValidIpOrUrl(ip))
                    errorMessages.Add($"Camera {cameraNumber}: Invalid or missing IP Address.");

                if (string.IsNullOrWhiteSpace(port) || !IsValidPort(port))
                    errorMessages.Add($"Camera {cameraNumber}: Invalid or missing RTSP Port.");

                if (string.IsNullOrWhiteSpace(username))
                    errorMessages.Add($"Camera {cameraNumber}: Username is required.");
            }

            // Validate only filled cameras
            ValidateCamera(Camera1IP.Text, Camera1Port.Text, Camera1Username.Text, 1);
            ValidateCamera(Camera2IP.Text, Camera2Port.Text, Camera2Username.Text, 2);
            ValidateCamera(Camera3IP.Text, Camera3Port.Text, Camera3Username.Text, 3);
            ValidateCamera(Camera4IP.Text, Camera4Port.Text, Camera4Username.Text, 4);

            if (errorMessages.Count > 0)
            {
                DisplayAlert("Validation Error", string.Join("\n", errorMessages), "OK");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Validates an IP address or URL format.
        /// </summary>
        private bool IsValidIpOrUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string pattern = @"^(https?:\/\/)?((\d{1,3}\.){3}\d{1,3}|([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,})$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// Validates RTSP port (must be numeric and between 1-65535).
        /// </summary>
        private bool IsValidPort(string input)
        {
            return int.TryParse(input, out int port) && port > 0 && port <= 65535;
        }

        /// <summary>
        /// Saves only the filled camera settings after validating them.
        /// </summary>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return; // Stop if validation fails

            try
            {
                int nextId = allSettings.Any() ? allSettings.Max(s => s.Id) + 1 : 1;

                CameraSettings newSettings = new CameraSettings
                {
                    Id = nextId,
                    Camera1_IP = string.IsNullOrWhiteSpace(Camera1IP.Text) ? null : Camera1IP.Text,
                    Camera1_Port = string.IsNullOrWhiteSpace(Camera1Port.Text) ? null : Camera1Port.Text,
                    Camera1_Username = string.IsNullOrWhiteSpace(Camera1Username.Text) ? null : Camera1Username.Text,
                    Camera1_Password = string.IsNullOrWhiteSpace(Camera1Password.Text) ? null : Camera1Password.Text,

                    Camera2_IP = string.IsNullOrWhiteSpace(Camera2IP.Text) ? null : Camera2IP.Text,
                    Camera2_Port = string.IsNullOrWhiteSpace(Camera2Port.Text) ? null : Camera2Port.Text,
                    Camera2_Username = string.IsNullOrWhiteSpace(Camera2Username.Text) ? null : Camera2Username.Text,
                    Camera2_Password = string.IsNullOrWhiteSpace(Camera2Password.Text) ? null : Camera2Password.Text,

                    Camera3_IP = string.IsNullOrWhiteSpace(Camera3IP.Text) ? null : Camera3IP.Text,
                    Camera3_Port = string.IsNullOrWhiteSpace(Camera3Port.Text) ? null : Camera3Port.Text,
                    Camera3_Username = string.IsNullOrWhiteSpace(Camera3Username.Text) ? null : Camera3Username.Text,
                    Camera3_Password = string.IsNullOrWhiteSpace(Camera3Password.Text) ? null : Camera3Password.Text,

                    Camera4_IP = string.IsNullOrWhiteSpace(Camera4IP.Text) ? null : Camera4IP.Text,
                    Camera4_Port = string.IsNullOrWhiteSpace(Camera4Port.Text) ? null : Camera4Port.Text,
                    Camera4_Username = string.IsNullOrWhiteSpace(Camera4Username.Text) ? null : Camera4Username.Text,
                    Camera4_Password = string.IsNullOrWhiteSpace(Camera4Password.Text) ? null : Camera4Password.Text
                };

                newSettings.SaveToFile();
                await DisplayAlert("Success", "Camera settings saved!", "OK");

                LoadSettings();
                await Shell.Current.GoToAsync("RTSPStreamPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
            }
        }
    }
}
