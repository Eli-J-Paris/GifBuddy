using Gif_Buddy.Interfaces;
using Gif_Buddy.Models;
using System.Text.Json;

namespace Gif_Buddy.Services
{
    public class GifApiService : IGifApiService
    {
        private static readonly HttpClient Client;

        static GifApiService()
        {
                                                                     //might screw us up later
            Client = new HttpClient() { BaseAddress = new Uri("https://localhost:7094") };
        }

        public async Task<List<Gif>> GetGifsAsync()
        {
            string url = "/Gifs";
            var result = new List<Gif>();
            var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var stringresponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<Gif>>(stringresponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result;
        }
    }
}
