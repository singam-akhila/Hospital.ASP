using Hospital.ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hospital.ASP.Controllers
{
    public class DoctorsController : Controller
    {
     
        private readonly IConfiguration _configuration;

           public DoctorsController(IConfiguration configuration)
         {
           _configuration = configuration;
         }

        public async Task<IActionResult> Index()
        {
            List<DoctorViewModel> doctors = new();
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Doctor/GetAllDoctor");
                if (result.IsSuccessStatusCode)
                {
                    doctors = await result.Content.ReadAsAsync<List<DoctorViewModel>>();
                }
            }
            return View(doctors);
        }


        public async Task<IActionResult> Details(int id)
        {
            DoctorViewModel doctor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctor/GetDoctorById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                }
            }
            return View(doctor);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Doctor/CreateDoctor", doctor);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                DoctorViewModel doctor = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Doctor/GetDoctorById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                        doctor.Specializations = await this.GetSpecializations();
                        return View(doctor);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Doctor doesn't exists");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Doctor/UpdateDoctor/{doctor.Id}", doctor);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            DoctorViewModel doctor = await this.GetDoctorById(id);
            if (doctor != null)
            {
                return View(doctor);
            }
            ModelState.AddModelError("", "Server Error . Please try later ");
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DoctorViewModel doctorVM)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Doctor/DeleteDoctor/{doctorVM.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            DoctorViewModel doctor = await this.GetDoctorById(doctorVM.Id);
            if (doctor != null)
            {
                return View(doctor);
            }
            ModelState.AddModelError("Delete", "Server Error. Please try later ");
            return View(doctor);
        }

        [NonAction]
        public async Task<DoctorViewModel> GetDoctorById(int id)
        {
            DoctorViewModel doctor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctor/GetDoctorById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                }
            }
            return (doctor);
        }

        [NonAction]
        public async Task<List<SpecializationViewModel>> GetSpecializations()
        {
            List<SpecializationViewModel> specializations = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Doctor/GetSpecializations");
                if (result.IsSuccessStatusCode)
                {
                    specializations = await result.Content.ReadAsAsync<List<SpecializationViewModel>>();
                }
            }
            return specializations;
        }


        [HttpGet]
        [Route("Doctors/Search/{specialization?}")]
        public async Task<IActionResult> Search(string specialization)
        {
            if (specialization != null)
            {
                List<DoctorViewModel> doctor = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Doctor/SearchDoctor/{specialization}");
                    if (result.IsSuccessStatusCode)
                    {
                        doctor = await result.Content.ReadAsAsync<List<DoctorViewModel>>();
                    }
                }
                return View(doctor);
            }
            return View();
        }





    }
}
