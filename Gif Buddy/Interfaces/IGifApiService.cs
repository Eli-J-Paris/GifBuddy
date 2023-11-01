using Gif_Buddy.Models;

namespace Gif_Buddy.Interfaces
{
    public interface IGifApiService
    {
        Task<List<Gif>> GetGifsAsync();
        Task<bool> PostGifAsync(string name, string url, int rating);
        Task<Gif> FindGifByIdAsync(int id);
    }
}
