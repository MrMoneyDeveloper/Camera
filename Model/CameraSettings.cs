using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Maui.Storage;

namespace Camera.Models
{
    public class CameraSettings
    {
        public int Id { get; set; }  // Unique ID to track each settings entry
        public string Camera1_IP { get; set; } = "";
        public string Camera1_Port { get; set; } = "";
        public string Camera1_Username { get; set; } = "";
        public string Camera1_Password { get; set; } = "";

        public string Camera2_IP { get; set; } = "";
        public string Camera2_Port { get; set; } = "";
        public string Camera2_Username { get; set; } = "";
        public string Camera2_Password { get; set; } = "";

        public string Camera3_IP { get; set; } = "";
        public string Camera3_Port { get; set; } = "";
        public string Camera3_Username { get; set; } = "";
        public string Camera3_Password { get; set; } = "";

        public string Camera4_IP { get; set; } = "";
        public string Camera4_Port { get; set; } = "";
        public string Camera4_Username { get; set; } = "";
        public string Camera4_Password { get; set; } = "";

        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "camera_settings.json");

        /// <summary>
        /// Saves new camera settings while preserving previous ones.
        /// </summary>
        public void SaveToFile()
        {
            try
            {
                // Ensure the directory exists before writing
                string directory = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine($"📁 Creating Directory: {directory}");
                    Directory.CreateDirectory(directory);
                }

                List<CameraSettings> settingsList = LoadAllSettings();

                // Assign a new ID based on the highest existing ID
                this.Id = settingsList.Any() ? settingsList.Max(s => s.Id) + 1 : 1;

                // Remove previous settings for the same Camera1_IP to avoid duplicates
                settingsList.RemoveAll(s => s.Camera1_IP == this.Camera1_IP);

                // Add the new settings entry
                settingsList.Add(this);

                // Serialize the settings list to JSON
                string json = JsonSerializer.Serialize(settingsList, new JsonSerializerOptions { WriteIndented = true });

                Console.WriteLine($"📄 Writing to JSON File: {FilePath}");

                File.WriteAllText(FilePath, json);

                // Confirm file creation
                if (File.Exists(FilePath))
                {
                    Console.WriteLine($"✅ File Successfully Created: {FilePath}");
                }
                else
                {
                    Console.WriteLine($"❌ File was NOT created!");
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"🚨 File I/O Error: {ioEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"🚨 JSON Serialization Error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error saving camera settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the most recent camera settings entry.
        /// </summary>
        public static CameraSettings LoadFromFile()
        {
            try
            {
                List<CameraSettings> settingsList = LoadAllSettings();

                // Return the most recent entry if available; otherwise, return default settings
                return settingsList.OrderByDescending(s => s.Id).FirstOrDefault() ?? new CameraSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error loading latest camera settings: {ex.Message}");
                return new CameraSettings();
            }
        }

        /// <summary>
        /// Loads all saved camera settings as a list.
        /// </summary>
        public static List<CameraSettings> LoadAllSettings()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    Console.WriteLine($"📂 Found JSON File: {FilePath}");

                    // Read the file safely
                    string json = File.ReadAllText(FilePath);

                    // Ensure the JSON isn't empty or just spaces
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            // Validate JSON format before deserialization
                            using (JsonDocument doc = JsonDocument.Parse(json))
                            {
                                return JsonSerializer.Deserialize<List<CameraSettings>>(json) ?? new List<CameraSettings>();
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            Console.WriteLine($"🚨 JSON Format Error: {jsonEx.Message}");
                            Console.WriteLine($"⚠️ Resetting corrupt JSON file...");

                            // Delete corrupt file and return a fresh settings list
                            File.Delete(FilePath);
                            return new List<CameraSettings>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ JSON File is Empty. Creating a new one...");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ JSON File NOT Found! Creating a new one.");
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"🚨 File I/O Error: {ioEx.Message}");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine($"🚨 Permission Error: {uaEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Unexpected Error loading camera settings: {ex.Message}");
            }

            // If there's an issue, return a new empty list
            return new List<CameraSettings>();
        }

    }
}
