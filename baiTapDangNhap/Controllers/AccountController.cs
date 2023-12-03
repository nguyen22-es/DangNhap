using baiTapDangNhap.baitap.data;

using baiTapDangNhap.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace baiTapDangNhap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DangNhapContext _dangNhapContext;

        public AccountController(DangNhapContext dangNhapContext)
        {
            _dangNhapContext = dangNhapContext;
        }

       

        // GET api/<AccountController>/5
        [HttpPost]
        public async Task<IActionResult> Post( LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userDangNhap = await _dangNhapContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
          
                if(userDangNhap != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,userDangNhap.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier, userDangNhap.UserId),


                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Ok();
                }
               
               


            }


            return BadRequest(ModelState);
        }

        // POST api/<AccountController>
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = model.Username,
                    Password = model.Password,
                    DisplayName = model.DisplayName

                };
             await    _dangNhapContext.AddAsync(user);
             await   _dangNhapContext.SaveChangesAsync();

                AuthenticationProperties props = null;
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                      

                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            }

            // Nếu ModelState không hợp lệ, quay lại trang đăng ký với dữ liệu đầu vào hiện tại
            return BadRequest(ModelState);


        }

    }
}
