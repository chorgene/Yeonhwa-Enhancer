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

        public static async Task UpdateHWID(string HWID, string password,double damageModifier)
        {
            string firebaseURL = $"https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users/{globalUsername}.json";
            // Create a new instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a sample data object to be posted
                    var data = new { HWID = HWID, password = globalPassword, damageMultiplier = damageModifier};

                    // Convert the data object to JSON
                    string jsonData = JsonSerializer.Serialize(data);

                    // Set the content type header to application/json
                    //client.DefaultRequestHeaders.Add("ContentType", "application/json");

                    // Post the data to the Firebase URL
                    HttpResponseMessage response = await client.PutAsync(firebaseURL, new StringContent(jsonData));

                    // Check if the response was successful
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
                Console.WriteLine(ex);
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
                GetModifier();
                await Authentication.UpdateHWID(Identifiers.GetHWID(), globalPassword, globalDamageModifier);
                return true;
            }
            else
            {
                if (HWID == Identifiers.GetHWID())
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
    }
}
