using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class MusicController : Controller
    {
        #region Properties & Constructors
        private readonly IConfiguration _config;

        private string BaseURL
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public MusicController(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddMusic()
        {
            var musicViewModel = new MusicViewModel();
            List<Artist> artistList = new List<Artist>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURL}Artist/Artists"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    artistList = JsonConvert.DeserializeObject<List<Artist>>(apiResponse);
                }
            }

            musicViewModel.ArtistList = new SelectList(artistList, "Id", "Name");

            return View(musicViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMusic(MusicViewModel musicViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var music = new Music()
                    {
                        ArtistId = int.Parse(musicViewModel.ArtistId),
                        Name = musicViewModel.Music.Name
                    };

                    // Verify athentication
                    string tokenJwt = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(tokenJwt))
                    {
                        ViewBag.ErrorMessage = "You must be authenticate";
                        return View(musicViewModel);
                    }

                    var stringData = JsonConvert.SerializeObject(music);
                    var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                    // Token in Header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenJwt);

                    var response = await httpClient.PostAsync($"{BaseURL}Music/CreateMusic", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.ErrorMessage = response.ReasonPhrase;

                    return View(musicViewModel);
                }
            }

            return View(musicViewModel);
        }
    }
}
