using baiTapDangNhap.ViewModel;
using clientDangNhap.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace clientDangNhap.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;


        public HomeController(HttpClient _httpClient)
        {
            this._httpClient = _httpClient;


        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult successfully()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
           //lóadfasdf

            var apiEndpoint = "https://localhost:7133/api/Account";

            var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


            var response = await _httpClient.SendAsync(request);
            

             if (response.IsSuccessStatusCode)
             {
                 // Đọc nội dung của phản hồi
                 var responseData = await response.Content.ReadAsStringAsync();
                 return Redirect("successfully");
             }
             else
             {
                  ViewBag( $"API request failed. Status code: {response.StatusCode}");

                return View("Error");
            }

           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}