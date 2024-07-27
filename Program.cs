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
            string baseUrl = "https://api.api-ninjas.com/v1/passwordgenerator";
            int passwordLength = 10;
            string url = $"{baseUrl}?length={passwordLength}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", "YOUR_API_KEY");

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var passwordResponse = JsonConvert.DeserializeObject<PasswordResponse>(responseBody);

                    Console.WriteLine($"Generated Password: {passwordResponse.Password}");
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
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
