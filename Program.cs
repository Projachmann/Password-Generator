using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Password_Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("WELCOME TO PASSWORD GENERATOR");
            Console.WriteLine("Get your api key from: https://api-ninjas.com/profile");
            Console.Write("Api Key: ");
            string apiKey = Console.ReadLine();

            Console.Clear();

            string baseUrl = "https://api.api-ninjas.com/v1/passwordgenerator";
            int passwordLength = 10;
            string url = $"{baseUrl}?length={passwordLength}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        var passwordResponse = JsonSerializer.Deserialize<PasswordResponse>(responseBody);

                        if (passwordResponse != null)
                        {
                            Console.WriteLine($"Generated Password: {passwordResponse.RandomPassword}");
                        }
                        else
                        {
                            Console.WriteLine("Failed to deserialize the response.");
                        }
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                        Console.WriteLine($"Response Body: {responseBody}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }
    }

    public class PasswordResponse
    {
        [JsonPropertyName("random_password")]
        public string RandomPassword { get; set; }
    }
}
