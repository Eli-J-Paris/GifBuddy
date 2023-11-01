using Gif_Buddy.Interfaces;
using Gif_Buddy.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
//using Newtonsoft.Json;
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
            Client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("Gif_Buddy", "1.0")
            );
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

        public async Task<Gif> FindGifByIdAsync(int id)
        {
            var gifs = await GetGifsAsync();
            foreach (var gif in gifs)
            {
                if (gif.Id == id)
                {
                    return gif;
                }
            }
            return new Gif();
        }

        public async Task<bool> PostGifAsync(string name, string url, int rating)
        {
            string apiEndpoint = "/Gifs";
            var gif = new Gif(){ Name = name, Url = url, Rating = rating};

            var jsonGifData = JsonSerializer.Serialize(gif);
            //var jsonGifData = new StringContent($"{{\"name\":\"{name}\",\"url\":\"{url}\",\"rating\":\"{rating}\"}}", Encoding.UTF8);
            HttpContent content = new StringContent(jsonGifData, Encoding.UTF8, "application/json");//possible change

            HttpResponseMessage response = await Client.PostAsync(apiEndpoint, content);
            
            if(response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            else
            {
                throw new HttpRequestException($"{response.ReasonPhrase}");
            }
        }
        public async Task<bool> PutGifAsync(Gif gif)
        {
            string apiEndpoint = $"/Gifs/{gif.Id}";
            var jsonGifData = JsonSerializer.Serialize(gif);

            HttpContent content = new StringContent(jsonGifData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(apiEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            else
            {
                throw new HttpRequestException($"{response.ReasonPhrase}");
            }
        }

        public async Task<bool> DeleteGifAsync(int id)
        {
            string apiEndpoint = $"/Gifs/{id}";
            var gif = await FindGifByIdAsync(id);
            var jsonGifData = JsonSerializer.Serialize(gif);

            HttpContent content = new StringContent(jsonGifData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.DeleteAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            else
            {
                throw new HttpRequestException($"{response.ReasonPhrase}");
            }

        }
    }
}
