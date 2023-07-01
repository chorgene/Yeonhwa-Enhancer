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

        /*public static async Task<string> Register(string username, string password)
        {
            string firebaseURL = "https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users.json";
            users user = new users();
            user.username = username;
            user.password = password;
            string data = JsonSerializer.Serialize(user);

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8);

                HttpResponseMessage response = await client.PostAsync(firebaseURL, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return $"HTTP {response.StatusCode} {response.ReasonPhrase}";
                }
            }
        }*/

        public static async Task<bool> Login(string username, string userPassword)
        {
            string firebaseURL = $"https://yeonhwa-enhancer-default-rtdb.asia-southeast1.firebasedatabase.app/users/{username}.json?print=pretty";
            bool authenticated;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(firebaseURL);
                    JsonDocument document = JsonDocument.Parse(response);
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
                        return true;
                    } 
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }

    public class users
    {
        public string password { get; set; }
    }
}
