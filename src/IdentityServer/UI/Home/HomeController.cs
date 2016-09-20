using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.Interfaces;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Infrastructure.Security;
using BitzerIoC.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Host.UI.Home
{
    /// <summary>
    /// Manage default index and Forget password requests
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IPasswordHasher<AspNetUser> _hasher;
        private readonly ILogger<HomeController> _logger;

        public HomeController( IIdentityRepository identityRepository,
                               IPasswordHasher<AspNetUser> hasher,
                               ILogger<HomeController> logger)
        {
            _identityRepository = identityRepository;
            _hasher = hasher;
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }


        #region Reset Password Action Methods
        /// <summary>
        /// ToDo: Pending Test
        /// ToDo: Need to perform client side validation using custom filter base libraray
        /// Return forgot password view which accept NewPasswordViewModel object 
        /// as ViewModal,This is Customized View
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="redirectUri">This returnUri is used When new user is created
        /// Or when Forget Password request is forwarded
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [Route("/ui/ResetPassword")]
        public IActionResult ResetPassword(string tokenKey,string redirectUri)
        {
            if (!string.IsNullOrEmpty(tokenKey) && _identityRepository.ValidateToken(tokenKey.Trim()))
            {
                ViewBag.FlagMessage = "Reset your password!";
            }
            else
            {
                return Redirect(Url.Content("login?forgotflag=error"));
            }

            return View(new NewPasswordViewModel() { ReturnUrl= redirectUri });
        }


        /// <summary>
        /// ToDo: Pending Test.
        /// Reset Password Action Method which take NewPasswordViewModel as Parameter
        /// update password in Db, redirect user to the url , wher he came from.
        /// </summary>
        /// <param name="newPasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/ui/ResetPassword")]
        public IActionResult ResetPassword(NewPasswordViewModel newPasswordViewModel)
        {
            string secureSalt = null;
            string hashedPassword = null;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(newPasswordViewModel.TokenKey))
                {
                    //Validate token
                    if (_identityRepository.ValidateToken(newPasswordViewModel.TokenKey.Trim()))
                    {
                        //Password Hashing
                        AspNetUser user = _identityRepository.GetUser(User.Identity.Name);
                        secureSalt = HashSecurity.GetSalt();
                        hashedPassword = _hasher.HashPassword(user, newPasswordViewModel.Password + secureSalt);

                        //Save Password
                        if (_identityRepository.SavePassword(newPasswordViewModel.TokenKey, hashedPassword, secureSalt, true))
                        {
                            return Redirect(GenericHelper.DecodeUrl(newPasswordViewModel.ReturnUrl));
                        }
                    }
                    //Invalid token
                    else
                    {
                        return Redirect(Url.Content("login?forgotflag=error"));
                    }
                }

                //If no redirection is performed then 
                _logger.LogError("Token key is empty or null");

            }
            else
            {
                ModelState.AddModelError("Error", "Please provide the Password and Confirm Password");
            }      
            return View(newPasswordViewModel);
        }
        #endregion
    }
}