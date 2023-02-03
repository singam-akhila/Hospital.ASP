using Hospital.ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hospital.ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = new();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("User/GetAllUser");
                if (result.IsSuccessStatusCode)
                {
                    users = await result.Content.ReadAsAsync<List<UserViewModel>>();
                }
            }
            return View(users);
        }


        public async Task<IActionResult> Details(int id)
        {
            UserViewModel user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"User/GetUserById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    user = await result.Content.ReadAsAsync<UserViewModel>();
                }
            }
            return View(user);
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("User/CreateUser", user);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
            }
           
            return View(user);
        }
    }
}
