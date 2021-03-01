using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class HomeController : Controller
    {
        #region Properties & Constructors
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        private string BaseURL
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        #endregion

        #region Functions
        public async Task<IActionResult> Index()
        {
            var listMusicViewModel = new ListMusicViewModel();
            var listMusics = new List<Music>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURL}Music/Musics"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listMusics = JsonConvert.DeserializeObject<List<Music>>(apiResponse);
                }
            }

            listMusicViewModel.ListMusic = listMusics;

            return View(listMusicViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
