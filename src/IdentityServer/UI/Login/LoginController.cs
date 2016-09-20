using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.Interfaces;
using BitzerIoC.Infrastructure.AppConstants;
using BitzerIoC.Infrastructure.Utilities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Services.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Host.UI.Login
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IUserInteractionService _interaction;
        private IHostingEnvironment _environment;


        public LoginController(LoginService loginService,IUserInteractionService interaction,
                               IHostingEnvironment environment)
        {
            _loginService = loginService;
            _interaction = interaction;
            _environment = environment;
  
        }


        [HttpGet("ui/login", Name = "Login")]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = new LoginViewModel();

            string message = HttpContext.Request.Query["forgotflag"].ToString();
           
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.FlagMessage = message;
            }
            //On the base of environment provide baseWebApiUrl
            ViewBag.BaseWebApiUrl = _environment.IsDevelopment() ? UrlConstants.WebapiBaseUrlDevelopment :UrlConstants.WebapiBaseUrlProduction;

            if (returnUrl != null)
            {
                var context = await _interaction.GetLoginContextAsync();
                if (context != null)
                {
                    vm.Username = context.LoginHint;
                    vm.ReturnUrl = returnUrl;
                }
            }

            return View(vm);
        }

        /// <summary>
        /// Validate user credentials and respond accordingly
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ui/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInputModel model)
        {
            var vm = new LoginViewModel(model);

            if (ModelState.IsValid)
            {
                if (_loginService.ValidateCredentials(model.Username, model.Password))
                {
                    if (!_loginService.isUserEnabled(model.Username))
                    {
                        ModelState.AddModelError("", "User is locked or not enabled yet (check you email).");
                    }
                    else
                    {
                        #region Get User, Issue Cookie and Redirect to the ReturnUrl (paramter)
                        try
                        {
                            //ToDo: Pending Functionality test
                            var user = _loginService.FindByUsername(model.Username);
                                                        
                            if (user != null)  //ToDo: Validation of roles, _loginService.validateUserRole(user)
                            {
                                await Task.Run(() => IssueCookie(user, "idsvr", "password"));

                                if (model.ReturnUrl != null && _interaction.IsValidReturnUrl(model.ReturnUrl))
                                {
                                    return Redirect(model.ReturnUrl);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(nameof(LoginController), "User is null");
                            LogHelper.WriteLog(nameof(LoginController), ex);
                        }
                        #endregion
                    }

                    ModelState.AddModelError("", "You can't login, please contact the administrator.");
                    return View(vm);
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(vm);
        }

        /// <summary>
        /// Function Issue cookie to user, Add Claims if required
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idp"></param>
        /// <param name="amr"></param>
        /// <returns></returns>
        private async Task IssueCookie(AspNetUser user, string idp,string amr)
        {
            //ToDo: Add User claims if required
            // user.UserClaims = _loginService.GetUserClaims(user.UserId);

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtClaimTypes.Subject, user.UserId));
            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, idp));
            claims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, DateTime.UtcNow.ToEpochTime().ToString()));
          
                       
            var ci = new ClaimsIdentity(claims, amr, JwtClaimTypes.Name, JwtClaimTypes.Role);
            var cp = new ClaimsPrincipal(ci);

            await HttpContext.Authentication.SignInAsync(Constants.DefaultCookieAuthenticationScheme, cp);
        }


        /// <summary>
        /// Function that perform some mapping,return JWT claims type like role,name etc.
        /// these types are very important because claims contains these name in token
        /// </summary>
        /// <param name="key">Key stored in Database</param>
        /// <returns></returns>
        public static string ReturnJwtClaimType(string key)
        {
            switch (key)
            {
                case "Name":
                    return JwtClaimTypes.Name;
                case "GivenName":
                    return JwtClaimTypes.GivenName;
                case "FamilyName":
                    return JwtClaimTypes.FamilyName;
                case "Email":
                    return JwtClaimTypes.Email;
                case "EmailVerified":
                    return JwtClaimTypes.EmailVerified;
                case "Role":
                    return JwtClaimTypes.Role;
                case "PhoneNumber":
                    return JwtClaimTypes.PhoneNumber;
                case "Address":
                    return JwtClaimTypes.Address;
                case "Guest":
                    return "guest";
                case "Installer":
                    return "installer";
                default:
                    return null;
            }
        }

    }
}
