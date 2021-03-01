using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class AccountController : Controller
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

        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var user = new User()
                    {
                        Username = loginViewModel.Username,
                        Password = loginViewModel.Password
                    };

                    var stringData = JsonConvert.SerializeObject(user);
                    var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{BaseURL}User/Authenticate", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        string stringJwt = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<JwtPayload>(stringJwt);
                        var token = jwt["token"].ToString();

                        HttpContext.Session.SetString("username", jwt["userName"].ToString());
                        HttpContext.Session.SetString("token", token);

                        ViewBag.Message = $"User logged In Successfully {jwt["userName"].ToString()}";
                    }

                    ViewBag.ErrorMessage = response.ReasonPhrase;

                    return View(loginViewModel);
                }
            }

            return View(loginViewModel);
        }

        [HttpPost]
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var register = new RegisterViewModel();
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var stringData = JsonConvert.SerializeObject(registerViewModel);
                    var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{BaseURL}User/Register", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        string stringJwt = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<JwtPayload>(stringJwt);
                        var token = jwt["token"].ToString();

                        HttpContext.Session.SetString("username", jwt["userName"].ToString());
                        HttpContext.Session.SetString("token", token);

                        ViewBag.Message = $"User logged In Successfully {jwt["userName"].ToString()}";

                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.ErrorMessage = response.ReasonPhrase;

                    return View(registerViewModel);
                }
            }

            return View(registerViewModel);
        }
    }
}
