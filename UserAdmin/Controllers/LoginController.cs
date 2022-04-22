using Data.Entities;
using Data.MMM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAdmin.Services;

namespace UserAdmin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userAPIClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var token = await _userAPIClient.Authenticate(request);
            if (token.ResultObj == null)
            {
                ModelState.AddModelError("", token.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(token.ResultObj);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
            HttpContext.Session.SetString("Token", token.ResultObj);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties
                );
            return RedirectToAction("Index", "Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
        [HttpGet]
        public ActionResult Fogot()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation(ForgotPassword forgotPassword)
        {
            return View(forgotPassword);
        }
        [HttpPost]
        public async Task<IActionResult> LON(InputModel Input)
        {
            if (ModelState.IsValid)
            {
                // Tìm user theo email gửi đến
                var kq = await _userAPIClient.GetTokenForgotPass(Input);

                /*var callbackUrl = Url.Action(
                    "/Login/ResetPasswordConfirm",
                    pageHandler: null,
                    values: new { email = Input.Email, token = kq.ResultObj },
                    protocol: Request.Scheme);*/
                var callbackUrl = Url.Action("ForgotPasswordConfirmation", "Login",
                    new { email = Input.Email, token = kq.ResultObj },Request.Scheme
                    );
                var str = "lay lai mat khau";
                await new EmailSender().SendEmailAsync(Input.Email, str, callbackUrl);
                return RedirectToAction("Text");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Text()
        {
            return View();
        }
       /* [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirm(string email,string token)
        {
            if(email==null || token == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var kq = await _userAPIClient.ResetPasswordConfirm(email,token);
            return RedirectToAction("ForgotPasswordConfirmation");
        }*/
        
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordConfirmation(string email,string token,string newpassword)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            else
            {
                if (email == null || token == null || newpassword == null)
                {
                    return View();
                }
                var kq = await _userAPIClient.ResetPasswordConfirm(email, token, newpassword);
                return RedirectToAction("Index");
            }
        }
    }
}
