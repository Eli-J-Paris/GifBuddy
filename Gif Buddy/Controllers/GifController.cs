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

        [Route("/gif/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var gif = await _gifApiService.FindGifByIdAsync(id);

            return View(gif);
        }

        [Route("/gif/{id}/update")]
        [HttpPost]
        public async Task<IActionResult> Update(Gif gif)
        {
            await _gifApiService.PutGifAsync(gif);
            return Redirect("/gif");
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
