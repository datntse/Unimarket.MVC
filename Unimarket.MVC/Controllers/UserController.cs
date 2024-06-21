using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Unimarket.MVC.Services.JwtTokenService;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Unimarket.MVC.Services;
using System.IdentityModel.Tokens.Jwt;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Helpers;
using System.Net;
using Unimarket.MVC.Models;

namespace Unimarket.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMailService _mailService;
        public UserController(ILogger<UserController> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IMailService mailService)
        {
            _mailService = mailService;
            _factory = httpClientFactory;
            _client = new HttpClient();
            _client = _factory.CreateClient("ServerApi");
            _client.BaseAddress = new Uri(configuration["Cron:localhost"]);
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return Redirect("/Home");
            }
            return View("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return Redirect("/Home");
            }
            ViewData["Message"] = TempData["Message"] as String;
            ViewData["ErrorMessage"] = TempData["ErrorMessage"] as string;
            var model = new LoginVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginDTO = new LoginDTO
            {
                UserName = model.UserName,
                Password = model.Password
            };

            // Send login request to Web API
            var response = await _client.PostAsync(
                _client.BaseAddress + "auth/signIn",
                new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                // Read response content
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                // Store token in session, cookie, or local storage
                HttpContext.Session.SetString("AccessToken", tokenResponse.Token);
                HttpContext.Session.SetString("RefeshToken", tokenResponse.RefreshToken);
                //HttpContext.Session.SetString("UserEmail", model.Email);
                // Redirect user to the home page or another appropriate page

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenResponse.Token);

                //var userId = await _currentUserService.User();
                //if (userId != null)
                //{
                //	HttpContext.Session.SetString("UserId", userId);
                //}
                var userId = token.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value).FirstOrDefault();
                var userFullName = token.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();

                if (userId != null)
                {
                    HttpContext.Session.SetString("UserId", userId);
                    HttpContext.Session.SetString("User_FullName", userFullName);
                }
                // Extract role claims
                var roleClaims = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                foreach (var role in roleClaims)
                {
                    if (role.Equals(AppRole.Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    } else if (role.Equals(AppRole.Staff))
                    {
                        return RedirectToAction("Index", "Order");
                    } 
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                ViewData["ErrorMessage"] = message;
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            // Send login request to Web API
            var response = await _client.GetAsync(
                _client.BaseAddress + $"auth/confirm?email={email}");
            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Xác thực tài khoản thành công!";
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            //string[] strings = model.Email.ToString().Split('@');
            //var isFptMail = strings.Length > 1 && strings[1].ToLower().Equals("fpt.edu.vn");

            //if(!isFptMail)
            //{
            //    TempData["ErrorMessage"] = "Bạn vui lòng sử dụng mail sinh viên của FPT";
            //    return RedirectToAction("Login", "User");
            //}


            var registerDTO = new RegisterDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                Password = model.Password,
                PhoneNumber = model.Phone,
                IsAdmin = false,
                DOB = model.DOB,
                Gender = model.Gender,
                Status = 1,
                StudentId = model.MSSV,
                Avatar = "unset",
                CCCDNumber = "unset",
            };
            var data = JsonConvert.SerializeObject(registerDTO);
            var response = await _client.PostAsync(_client.BaseAddress + "auth/signUp",
                new StringContent(
                    data,
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var url = Url.Action("ConfirmEmail", "User", new { email = model.Email }, protocol: Request.Scheme);
                await _mailService.SendEmailAsync(model.Email, "Xác thực tài khoản của bạn", url);
                TempData["Message"] = "Đến email dể xác nhận tài khoản";
                return RedirectToAction("Login", "User");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = errorResponse;
                return RedirectToAction("Login", "User");
            }
        }
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLogin()
        //{
        //    var props = new AuthenticationProperties { RedirectUri = "/user/GoogleLogin" };
        //    return Challenge(props, GoogleDefaults.AuthenticationScheme);
        //}

        //public async Task<ActionResult> GoogleLogin()
        //{
        //    var responseGoogle = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    if (responseGoogle.Principal == null) return BadRequest();
        //    var name = responseGoogle.Principal.FindFirstValue(ClaimTypes.Name);
        //    var givenName = responseGoogle.Principal.FindFirstValue(ClaimTypes.GivenName);
        //    var email = responseGoogle.Principal.FindFirstValue(ClaimTypes.Email);
        //    //Do something with the claims
        //    // var user = await UserService.FindOrCreate(new { name, givenName, email});
        //    var user = new RegisterDTO
        //    {
        //        FirstName = name,
        //        LastName = name,
        //        Email = email,
        //        Password = email
        //    };

        //    // Send login request to Web API
        //    var response = await _client.PostAsync(
        //        _client.BaseAddress + "User/GoogleLogin",
        //        new StringContent(
        //            JsonConvert.SerializeObject(user),
        //            Encoding.UTF8,
        //            "application/json"));

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Read response content
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

        //        // Store token in session, cookie, or local storage
        //        HttpContext.Session.SetString("AccessToken", tokenResponse.Token);
        //        HttpContext.Session.SetString("RefeshToken", tokenResponse.RefreshToken);
        //        HttpContext.Session.SetString("UserEmail", email);
        //        HttpContext.Session.SetString("FirstName", name);
        //        // Redirect user to the home page or another appropriate page

        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
        //        var currentUser = await _currentUserService.User();
        //        if (user != null)
        //        {
        //            HttpContext.Session.SetString("UserId", currentUser.Id.ToString());
        //        }

        //        var handler = new JwtSecurityTokenHandler();
        //        var token = handler.ReadJwtToken(tokenResponse.Token);

        //        // Extract role claims
        //        var roleClaims = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        //        foreach (var role in roleClaims)
        //        {
        //            if (role.Equals(AppRole.Admin))
        //            {
        //                // Dashboard
        //                return RedirectToAction("Index", "Dashbroad");
        //            }
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewData["ErrorMessage"] = "not validate";
        //        //ModelState.AddModelError(string.Empty, "Invalid username or password");
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return RedirectToAction("Login", "User");
        //}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var response = await _client.DeleteAsync(_client.BaseAddress + "auth/signOut");
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session?.Remove("AccessToken");
                HttpContext.Session?.Remove("RefeshToken");
                HttpContext.Session?.Remove("UserId");
                return RedirectToAction("Login");
            }
            return Unauthorized();
        }
    }
}
