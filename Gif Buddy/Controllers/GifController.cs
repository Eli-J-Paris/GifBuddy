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

            ViewData["Success"] = TempData["Success"];
            return View(gifs);
        }

        [Route("/gifs/new")]
        public IActionResult NewGif()
        {
            return View();
        }

        [HttpPost]
        [Route("/CreateGif")]
        public async Task<IActionResult> CreateGif(Gif gif)
        {
            if(ModelState.IsValid)
            {
                await _gifApiService.PostGifAsync(gif.Name, gif.Url, gif.Rating);
                TempData["Success"] = "Success!";

                return Redirect("/gif");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
