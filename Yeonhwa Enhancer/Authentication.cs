using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Yeonhwa_Enhancer
{
    public class Authentication
    {
        public static JsonDocument document;
        public static string globalUsername;
        public static string globalPassword;
        public static double globalDamageModifier;

        static async Task UpdateHWID(string HWID)
        {
            string firebaseURL = $"https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users/{globalUsername}.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var data = new { HWID = HWID};

                    string jsonData = JsonSerializer.Serialize(data);

                    HttpResponseMessage response = await client.PatchAsync(firebaseURL, new StringContent(jsonData));

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("First Time Login!");
                    }
                    else
                    {
                        Console.WriteLine("Error posting data: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        public static async Task<bool> Login(string username, string userPassword)
        {

            globalUsername = username;
            globalPassword = userPassword;

            bool authenticated;

            string firebaseURL = $"https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users/{username}.json?print=pretty";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(firebaseURL);
                    document = JsonDocument.Parse(response);
                    JsonElement root = document.RootElement;
                    JsonElement passwordElement = root.GetProperty("password");
                    string password;

                    switch (passwordElement.ValueKind)
                    {
                        case JsonValueKind.Number:
                            password = passwordElement.GetDouble().ToString();
                            break;
                        case JsonValueKind.String:
                            password = passwordElement.GetString();
                            break;
                        default:
                            throw new InvalidOperationException($"Unexpected value kind {passwordElement.ValueKind}.");
                    }
                    if (password == userPassword)
                    {
                        if (await AuthHWID(document) == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> AuthHWID(JsonDocument document)
        {
            JsonElement root = document.RootElement;
            JsonElement HWIDElement = root.GetProperty("HWID");
            string HWID;

            switch (HWIDElement.ValueKind)
            {
                case JsonValueKind.Number:
                    HWID = HWIDElement.GetDouble().ToString();
                    break;
                case JsonValueKind.String:
                    HWID = HWIDElement.GetString();
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected value kind {HWIDElement.ValueKind}.");
            }

            if (string.IsNullOrEmpty(HWID))
            {
                await Authentication.UpdateHWID(GetHWID());
                return true;
            }
            else
            {
                if (HWID == GetHWID())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Something went wrong, please contact developer");
                    return false;
                }
            }
        }

        public static double GetModifier()
        {
            JsonElement root = document.RootElement;
            JsonElement damageElement = root.GetProperty("damageMultiplier");
            double damage;

            damage = damageElement.GetDouble();
            globalDamageModifier = damage;
            return damage;
        }

        public static string GetHWID()
        {
            string registryPath = @"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001";
            string valueName = "HwProfileGuid";
            string hwProfileGuid = "";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    object value = key.GetValue(valueName);
                    if (value != null)
                    {
                        hwProfileGuid = value.ToString();
                    }
                }
            }
            return hwProfileGuid;
        }

        public static async Task UpdateLastLogin()
        {
            string firebaseURL = $"https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users/{globalUsername}.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    var data = new { lastLogin = $"{now.Day}{GetDaySuffix(now.Day)} {now.ToString("MMMM yyyy")} {now.ToString("hh:mm:ss")} {now.ToString("tt")}" };

                    string jsonData = JsonSerializer.Serialize(data);

                    HttpResponseMessage response = await client.PatchAsync(firebaseURL, new StringContent(jsonData));

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: ");
                }
            }
        }

        public static string GetDaySuffix(int day)
        {
            if (day >= 11 && day <= 13)
            {
                return "th";
            }

            switch (day % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
