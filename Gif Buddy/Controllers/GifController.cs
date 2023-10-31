using Gif_Buddy.Interfaces;
using Gif_Buddy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gif_Buddy.Controllers
{
    public class GifController : Controller
    {
        private readonly IGifApiService _gifApiService;

        public GifController(IGifApiService gifApiService)
        {
            _gifApiService = gifApiService;
        }
        public async Task<IActionResult> Index()
        {

            List<Gif> gifs = new List<Gif>();
            gifs = await _gifApiService.GetGifsAsync();
            
            return View(gifs);
        }
    }
}
