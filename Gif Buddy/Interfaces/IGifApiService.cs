using Gif_Buddy.Models;

namespace Gif_Buddy.Interfaces
{
    public interface IGifApiService
    {
        Task<List<Gif>> GetGifsAsync();

    }
}
