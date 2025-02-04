using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace Camera.Models
{
    public class CameraSettings
    {
        public int Id { get; set; }
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
        private static readonly string EncryptionKey = "YOUR_SECRET_KEY_32CHARS_LONG"; // CHANGE THIS TO A SECURE KEY!

        /// <summary>
        /// Encrypts and saves camera settings, ensuring previous data is not lost.
        /// </summary>
        public void SaveToFile()
        {
            try
            {
                List<CameraSettings> settingsList = LoadAllSettings();

                // Find existing settings (if any)
                var existingSettings = settingsList.FirstOrDefault(s => s.Id == this.Id);

                if (existingSettings != null)
                {
                    // Update only the fields that have values
                    existingSettings.Camera1_IP = !string.IsNullOrWhiteSpace(this.Camera1_IP) ? this.Camera1_IP : existingSettings.Camera1_IP;
                    existingSettings.Camera1_Port = !string.IsNullOrWhiteSpace(this.Camera1_Port) ? this.Camera1_Port : existingSettings.Camera1_Port;
                    existingSettings.Camera1_Username = !string.IsNullOrWhiteSpace(this.Camera1_Username) ? this.Camera1_Username : existingSettings.Camera1_Username;
                    existingSettings.Camera1_Password = !string.IsNullOrWhiteSpace(this.Camera1_Password) ? this.Camera1_Password : existingSettings.Camera1_Password;

                    existingSettings.Camera2_IP = !string.IsNullOrWhiteSpace(this.Camera2_IP) ? this.Camera2_IP : existingSettings.Camera2_IP;
                    existingSettings.Camera2_Port = !string.IsNullOrWhiteSpace(this.Camera2_Port) ? this.Camera2_Port : existingSettings.Camera2_Port;
                    existingSettings.Camera2_Username = !string.IsNullOrWhiteSpace(this.Camera2_Username) ? this.Camera2_Username : existingSettings.Camera2_Username;
                    existingSettings.Camera2_Password = !string.IsNullOrWhiteSpace(this.Camera2_Password) ? this.Camera2_Password : existingSettings.Camera2_Password;

                    existingSettings.Camera3_IP = !string.IsNullOrWhiteSpace(this.Camera3_IP) ? this.Camera3_IP : existingSettings.Camera3_IP;
                    existingSettings.Camera3_Port = !string.IsNullOrWhiteSpace(this.Camera3_Port) ? this.Camera3_Port : existingSettings.Camera3_Port;
                    existingSettings.Camera3_Username = !string.IsNullOrWhiteSpace(this.Camera3_Username) ? this.Camera3_Username : existingSettings.Camera3_Username;
                    existingSettings.Camera3_Password = !string.IsNullOrWhiteSpace(this.Camera3_Password) ? this.Camera3_Password : existingSettings.Camera3_Password;

                    existingSettings.Camera4_IP = !string.IsNullOrWhiteSpace(this.Camera4_IP) ? this.Camera4_IP : existingSettings.Camera4_IP;
                    existingSettings.Camera4_Port = !string.IsNullOrWhiteSpace(this.Camera4_Port) ? this.Camera4_Port : existingSettings.Camera4_Port;
                    existingSettings.Camera4_Username = !string.IsNullOrWhiteSpace(this.Camera4_Username) ? this.Camera4_Username : existingSettings.Camera4_Username;
                    existingSettings.Camera4_Password = !string.IsNullOrWhiteSpace(this.Camera4_Password) ? this.Camera4_Password : existingSettings.Camera4_Password;
                }
                else
                {
                    // Add new settings entry
                    this.Id = settingsList.Any() ? settingsList.Max(s => s.Id) + 1 : 1;
                    settingsList.Add(this);
                }

                // Encrypt and save
                string json = JsonSerializer.Serialize(settingsList, new JsonSerializerOptions { WriteIndented = true });
                string encryptedJson = Encrypt(json, EncryptionKey);
                File.WriteAllText(FilePath, encryptedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error saving encrypted camera settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the latest saved camera settings and decrypts them.
        /// </summary>
        public static CameraSettings LoadFromFile()
        {
            try
            {
                List<CameraSettings> settingsList = LoadAllSettings();
                return settingsList.OrderByDescending(s => s.Id).FirstOrDefault() ?? new CameraSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error decrypting camera settings: {ex.Message}");
                return new CameraSettings();
            }
        }

        /// <summary>
        /// Loads all saved camera settings and decrypts them.
        /// </summary>
        public static List<CameraSettings> LoadAllSettings()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string encryptedJson = File.ReadAllText(FilePath);
                    string decryptedJson = Decrypt(encryptedJson, EncryptionKey);

                    return JsonSerializer.Deserialize<List<CameraSettings>>(decryptedJson) ?? new List<CameraSettings>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error decrypting settings file: {ex.Message}");
            }
            return new List<CameraSettings>();
        }

        /// <summary>
        /// Encrypts a string using AES-256.
        /// </summary>
        private static string Encrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts an AES-256 encrypted string.
        /// </summary>
        private static string Decrypt(string cipherText, string key)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = new byte[16];

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error decrypting data: {ex.Message}");
                return "";
            }
        }
    }
}
