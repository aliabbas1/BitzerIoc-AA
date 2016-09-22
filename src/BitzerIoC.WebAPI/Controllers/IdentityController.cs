using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.DTO;
using BitzerIoC.Infrastructure.AppConstants;
using System;
using BitzerIoC.Infrastructure.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using BitzerIoC.Infrastructure.Security;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace BitzerIoC.WebAPI.Controllers
{
    //ToDo: Need to update Api Controller Code
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityRepository _identityRepository;
        private readonly IHostingEnvironment _enviroment;
        private readonly ILogger<IdentityController> _logger;
        private readonly IPasswordHasher<AspNetUser> _hasher;

        public IdentityController(IdentityRepository identityRepository,
                                  IHostingEnvironment environment,
                                  ILogger<IdentityController> logger,
                                  IPasswordHasher<AspNetUser> hasher)
        {
            _identityRepository = identityRepository;
            _enviroment = environment;
            _logger = logger;
            _hasher = hasher;
        }
        

        #region Get urls on environment base
        private string baseIdentityServerUrl
        {
            get
            {
                return _enviroment.IsDevelopment() ? UrlConstants.IdentityServerBaseUrlDevelopment : UrlConstants.IdentityServerBaseUrlProduction;
            }
        }
       #endregion


        #region Roles
        /// <summary>
        /// Get collection of RoleDTO objects
        /// </summary>
        /// <returns>RoleDTO collection</returns>
        [HttpGet]
        public IEnumerable<AspNetRolesDTO> GetRoles()
        {
            IEnumerable<AspNetRolesDTO> roles = _identityRepository.GetRolesDTO();
            return roles;

        }

        /// <summary>
        /// Get the User with role info, return collection of UserDetailDTO object 
        /// </summary>
        /// <param name="boundaryId">BoundaryId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsersWithRoles")]
        public IEnumerable<UserDetailDTO> GetUsersWithRoles(int boundaryId)
        {
            IEnumerable<UserDetailDTO> roles = _identityRepository.GetUsersWithRoles(boundaryId);
            return roles;

        }
        #endregion

        /// <summary>
        /// ToDo: Pending Test
        /// Function take email as parameter and return true if it exist in system
        /// </summary>
        /// <param name="username">username is email address</param>
        /// <returns>bool</returns>
        /// PR version: 1.2
        [HttpPost]
        public bool ForgotPassword(string username,string returnUrl)
        {
            #region Variables
            EmailHelper helper;
            bool IsComplete = false;
            bool status = false;
            string UserName = null;
            string callbackUrl = null;
            string htmlmessage = null;
            string Token = Guid.NewGuid().ToString();
            string redirectUri = null;
            bool ValidEmail = false;
            DateTime ExpiryDate; 
            string loginUrl = null;
            #endregion

            try
            {
                #region Initialization           
                  redirectUri = GenericHelper.EncodeUrl(GetReturnUri(returnUrl));
                  ValidEmail = _identityRepository.ValidateEmail(username.Trim());            
                  ExpiryDate = DateTime.Now.AddDays(1);
                  loginUrl = baseIdentityServerUrl + "/ui/login";         
                #endregion
               
                if (ValidEmail)
                {
                    UserName = _identityRepository.GetUserNameByEmail(username.Trim());
                    if (_identityRepository.SaveToken(username.Trim(), Token, ExpiryDate, IsComplete))
                    {
                        callbackUrl = baseIdentityServerUrl + "/ui/ResetPassword?TokenKey=" + Token + "&redirectUri=" + redirectUri;

                        htmlmessage = String.Format("<b>Hi {0}.</b><br/><br/>Please click<b> <a href='{1}'> here </a></b>" +
                                      "to reset your password. <br/><br/>The link is valid for 24 hours.<br/><br/><br/><b>If you did NOT request a new password," +
                                      "do not click on the link. </b><br/><br/>You can access the Remote Caretaking system <a href='{2}'> here. </a>",
                                      UserName, callbackUrl, loginUrl);

                        helper = new EmailHelper(username.Trim(), EmailConstants.From, "Reset password request", "", htmlmessage, null);
                        status = helper.SendEmailAsync().Result;
                     }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: Forgot password action not completed --> " + ex.Message);
                throw ex;
            }

            return status;
         
        }

        #region User Profile

        /// <summary>
        /// ToDo: Update it to POST (Pirority :: High)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// PR version 1.1
        [HttpGet]
        [Route("ValidateCredentials")]
        public bool ValidateCredentials(string username, string password)
        {
            AspNetUser user = _identityRepository.GetUser(username);
            if (user != null)
            {
                // 5 CompareTo(6) = -1      First int is smaller.
                // 6 CompareTo(5) =  1      First int is larger.
                // 5 CompareTo(5) =  0      Ints are equal.
               PasswordVerificationResult result = _hasher.VerifyHashedPassword(user, user.Password, password + user.HashSalt);
                if (result.CompareTo(PasswordVerificationResult.Success) == 0)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// update password of user in profile page
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if password successfully updated</returns>
        /// PR version 1.1

        [HttpPost]
        [Authorize]
        [Route("UpdatePassword")]       
        public bool UpdatePassword(string username, string password)
        {
            #region variables
            EmailHelper helper;
            bool isEmailSent = false;
            string secureHashSalt = null;
            string hashedPassword = null;
            bool isPasswordUpdated = false;
            string UserName = null;
            string htmlmessage = null;
            #endregion

            secureHashSalt = HashSecurity.GetSalt();
            AspNetUser user   = _identityRepository.GetUser(username);
            hashedPassword    = _hasher.HashPassword(user, secureHashSalt);
            isPasswordUpdated = _identityRepository.UpdatePassword(username, hashedPassword, secureHashSalt);

            /// if password is successfully updated then send email to user that your profile has been updated.
            /// PR version 1.0
            /// return send email status, true of false.
            if (isPasswordUpdated)
            {
                #region Send Confirmation Email to User 
                UserName = _identityRepository.GetUserNameByEmail(username.Trim());
                htmlmessage = String.Format("<b>Hi {0}.</b><br/><br/>Your profile " + username + " has been successfully updated at " + DateTime.Today.ToString("d") + "." +
                                   "<br/><br/><b>Don't recognize this activity?</b> <br/>Admin: tau@lodam.com <br/><br/>" +
                                   "Regards <br/> BitzerIoC Support", UserName);

                helper = new EmailHelper(username.Trim(), EmailConstants.From, "BitzerIoc profile updated.", "", htmlmessage, null);
                return isEmailSent = helper.SendEmailAsync().Result;
                #endregion
            }
            return isPasswordUpdated;
        }

        /// <summary>
        /// update name of user in profile page
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <returns>true if name successfully updated</returns>
        /// PR version 1.0
        [HttpPost]
        [Authorize]
        [Route("UpdateName")]
        public bool UpdateName(string username, string name)
        {
            #region variables
            EmailHelper helper;
            bool isEmailSent = false;
            bool isNameUpdated = false;
            string UserName = null;
            string htmlmessage = null;
            #endregion

            isNameUpdated = _identityRepository.UpdateName(username, name);

            /// if name is successfully updated then send email to user that your profile has been updated.
            /// Author: Ali Abbas, version 1.0
            /// return send email status, true of false.
            if (isNameUpdated)
            {
                #region Send Confirmation Email to User 
                UserName = _identityRepository.GetUserNameByEmail(username.Trim());
                htmlmessage = String.Format("<b>Hi {0}.</b><br/><br/>Your profile " + username + " has been successfully updated at " + DateTime.Today.ToString("d") + "." +
                                   "<br/><br/><b>Don't recognize this activity? </b><br/>Admin: tau@lodam.com <br/><br/>" +
                                   "Regards <br/> BitzerIoC Support", UserName);

                helper = new EmailHelper(username.Trim(), EmailConstants.From, "BitzerIoc profile updated.", "", htmlmessage, null);
                return isEmailSent = helper.SendEmailAsync().Result;
                #endregion
            }
            return isNameUpdated;
        }

        /// <summary>
        /// Get the user profile information by email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserProfileByUsername")]
        public UserDetailDTO GetUserProfileByUsername(string email,int boundaryId)
        {
            UserDetailDTO user = _identityRepository.GetUserProfileByUsername(email, boundaryId);
            return user;
        }
        #endregion

        #region Helper Method
        /// <summary>
        /// ToDo: Pending Test
        /// Get collection of querystring
        /// Step 1: Extract Querystrings,
        /// Step 2: Get return uri,
        /// Step 3: Remove a string from returnurl,
        /// Step 4 Return clean url.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private static string GetReturnUri(string returnUrl)
        {
            //Get redirectUri
            string redirectUri = null;
            Dictionary<string, string> queryStringCollection = GenericHelper.ParseQueryString(returnUrl);
            queryStringCollection.TryGetValue("redirect_uri", out redirectUri);

            //Cleanup redirectUri
            string removeString = "signin-oidc";
            int index = redirectUri.IndexOf(removeString);
            string cleanUrl = (index < 0)? redirectUri:redirectUri.Remove(index, removeString.Length);
           

            return cleanUrl;
        }
        #endregion

        /// <summary>
        /// verify that is email already exist or not at user creation time. if email is already exist in db 
        /// then return user complete info 
        /// </summary>
        /// <param name="UserEmail"></param>
        /// <param name="boundaryId"></param>
        /// <returns>UserDetailDTO</returns>
        /// Author: Ali Abbas , version 1.0
        [HttpGet]
        [Route("AlreadyExistEmail")]
        public UserDetailDTO AlreadyExistEmail(string UserEmail, int boundaryId)
        {
            var IsValid = _identityRepository.ValidateEmail(UserEmail);
            UserDetailDTO user = null;
            if (IsValid)
            {
                user = _identityRepository.GetUserProfileByUsername(UserEmail, boundaryId);
            }
            return user;
        }
    }
}
