using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorLib
{
    public static class ConfigManager
    {
        #region Private Fields



        #endregion



        #region Public Properties

        public static string WorkingDirectory { get; set; }

        #endregion



        static ConfigManager()
        {
            // Set working directory
            WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),Application.Current.MainWindow.GetType().Assembly.GetName().Name, "configdata");
            if (!Directory.Exists(WorkingDirectory)) Directory.CreateDirectory(WorkingDirectory);
            Console.WriteLine("WorkingDirectory=" + WorkingDirectory);
        }



        #region Private Methods



        #endregion



        #region Public Methods

        public static void Save(ConfigGroup configGroup)
        {
            try
            {
                var json = JsonConvert.SerializeObject(configGroup, Formatting.Indented, new SecureStringSerializer());
                File.WriteAllText(Path.Combine(WorkingDirectory, configGroup.DisplayName + ".config"), json);

                // Mark as initialized after saving
                configGroup.IsInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static T Load<T>(string displayName)
        {
            try
            {
                return (T) Convert.ChangeType(Load(displayName, typeof (T)), typeof (T));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }

        public static ConfigGroup Load(string displayName, Type targetType)
        {
            try
            {
                // Make file path
                var filePath = Path.Combine(WorkingDirectory, displayName + ".config");

                // Check if file exists
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Config file not found at " + filePath);

                // Read file
                using (var file = File.OpenText(filePath))
                {
                    // Deserialize
                    var serializer = new JsonSerializer();
                    serializer.Converters.Add(new SecureStringSerializer());
                    var value = serializer.Deserialize(file, targetType);

                    // Mark as initialized after loading
                    var configGroup = value as ConfigGroup;
                    if (configGroup != null)
                    {
                        configGroup.IsInitialized = true;
                    }

                    return configGroup;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        #endregion
    }

    public class SecureStringSerializer : JsonConverter
    {
        private static readonly byte[] SAditionalEntropy = {4, 5, 3, 7, 8, 9, 4, 3, 4, 3, 1};


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ss = value as SecureString;
            writer.WriteStartObject();
            writer.WritePropertyName("password");
            serializer.Serialize(writer, ProtectPassword(ConvertToUnsecureString(ss)));
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            return ConvertToSecureString(UnprotectPassword(properties[0].Value.ToString()));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (SecureString);
        }

        private string ProtectPassword(string password)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted 
                //  only by the same current user. 
                return Convert.ToBase64String(ProtectedData.Protect(GetBytes(password),
                    SAditionalEntropy,
                    DataProtectionScope.CurrentUser));
            }
            catch (Exception e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e);
                return null;
            }
        }

        private string UnprotectPassword(string encryptedPassword)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser. 
                return GetString(ProtectedData.Unprotect(Convert.FromBase64String(encryptedPassword),
                    SAditionalEntropy, DataProtectionScope.CurrentUser));
            }
            catch (Exception e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e);
                return null;
            }
        }

        private byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length*sizeof (char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length/sizeof (char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null) return "";

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private SecureString ConvertToSecureString(string password)
        {
            if (string.IsNullOrEmpty(password)) return new SecureString();

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }
}
