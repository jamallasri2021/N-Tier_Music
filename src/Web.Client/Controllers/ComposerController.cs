using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class ComposerController : Controller
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

        public ComposerController(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var composerViewModel = new ListComposerViewModel();

            var listComposers = new List<Composer>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURL}Composer/Composers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listComposers = JsonConvert.DeserializeObject<List<Composer>>(apiResponse);
                }
            }

            composerViewModel.ListComposer = listComposers;

            return View(composerViewModel);
        }

        [HttpGet]
        public IActionResult AddComposer()
        {
            ComposerViewModel composerViewModel = new ComposerViewModel();

            return View(composerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddComposer(ComposerViewModel composerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var composer = new Composer()
                    {
                        FirstName = composerViewModel.FirstName,
                        LastName = composerViewModel.LastName
                    };

                    var stringData = JsonConvert.SerializeObject(composer);
                    var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{BaseURL}Composer/CreateComposer", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Composer");

                    }

                    ViewBag.ErrorMessage = response.ReasonPhrase;

                    return View(composerViewModel);
                }
            }

            return View(composerViewModel);
        }
    }
}
