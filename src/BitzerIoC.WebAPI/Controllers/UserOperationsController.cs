using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Infrastructure.Utilities;
using BitzerIoC.Infrastructure.AppConstants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using BitzerIoC.Infrastructure.Filters;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BitzerIoC.WebAPI.Controllers
{
    /// <summary>
    /// Manage the user related operations
    /// </summary>
    [Route("api/[controller]")]
    //[Authorize] 
    //[AdminAccessFilter]
    public class UserOperationsController : ControllerBase
    {

        private readonly IdentityRepository _identityRepository;
        private readonly ILogger<UserOperationsController> _logger;
        private readonly IHostingEnvironment _environment;
        #region Environment base urls
        /// <summary>
        /// Identity Server Url on the base of environement
        /// </summary>
        private string baseUrlIdentityServer
        {
            get
            {
                return _environment.IsDevelopment() ? UrlConstants.IdentityServerBaseUrlDevelopment : UrlConstants.IdentityServerBaseUrlProduction;
            }
        }
        /// <summary>
        /// BitzerIoC main url base on environment
        /// </summary>
        private string baseUrlBitzerIoC
        {
            get
            {
                return _environment.IsDevelopment() ? UrlConstants.BitzerIoCBaseUrlDevelopment : UrlConstants.BitzerIoCBaseUrlProduction;
            }

        }
        /// <summary>
        /// Base url of Admin
        /// </summary>
        private string baseUrlBitzerIoCAdmin
        {
            get
            {
                return _environment.IsDevelopment() ? UrlConstants.BitzerIoCAdminBaseUrlDevelopment : UrlConstants.BitzerIoCAdminBaseUrlProduction;
            }

        }
        #endregion

        public UserOperationsController(IdentityRepository identityRepository, ILogger<UserOperationsController> logger,
                                        IHostingEnvironment environment)
        {
            _identityRepository = identityRepository;
            _environment = environment;
			 _logger = logger;
        }


        #region Get
        /// <summary>
        /// Get the user by userId
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        /// <uri>api/UserOperations/{userId-parameter}</uri>
        [HttpGet("{userId}")]
        public async Task<AspNetUser> GetUser(string userId)
        {
            AspNetUser user = await Task.Run(()=> _identityRepository.GetUserById(userId));
            return user;
        }
        
        /// <summary>
        /// Get the user boundaries, return list of user boundaries
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of User Boundaries</returns>
        /// <uri>api/UserOperations/GetUserBoundaries/{userId-parameter}</uri>
        [HttpGet]
        [Route("GetUserBoundaries/{userId}")]
        public async Task<List<UserBoundary>> GetUserBoundaries(string userId)
        {
            List<UserBoundary> userBoundaries = await Task.Run(() => _identityRepository.GetUserBoundaries(null,userId:userId));
            return userBoundaries;
        }
        #endregion

        #region Delete 
        /// <summary>
        /// Delete the user along with its child record,
        /// User, UserClaims,UserDevices,UserBoundary,UserRole,
        /// return tru if success
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        /// <uri>api/UserOperations/{userId-parameter}</uri>
        [HttpDelete("{userId}")]
        public async Task<bool> DeleteUser(string userId)
        {
            bool status = await Task.Run(() =>_identityRepository.DeleteUser(userId));
            return status;
        }

        /// <summary>
        /// Delete user boundary,User role associated with boundary, Return true if successfull
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="boundaryId">Targeted Boundary Id</param>
        /// <returns>bool</returns>
        /// <uri>api/UserOperations/{userId-parameter}/{boundaryId:int-parameter}</uri>
        [HttpDelete("{userId}/{boundaryId:int}")]
        public async Task<bool> DeleteUserBoundary(string userId,int boundaryId)
        {
            bool status = await Task.Run(()=>_identityRepository.DeleteUserBoundary(userId,boundaryId));
            return status;
        }
        #endregion

        #region Add,Update User 
         /// <summary>
        /// ToDo: Test pending
        /// Create/Update User, If user id is not null than user updated else user created and send email
        /// for account activation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userEmail"></param>
        /// <param name="name"></param>
        /// <param name="userPhone"></param>
        /// <param name="roleId"></param>
        /// <param name="oldRoleId"></param>
        /// <param name="isEnable"></param>
        /// <returns>bool value</returns>
        /// Author : khurram shehzad,Ali abbas
        /// Version 2.0
        [HttpPost]
        [Route("{userId}/{userEmail}/{name}/{userPhone}/{roleId}/{oldRoleId}/{isEnable}")]
        public bool SaveUser(string userId, string userEmail, string name, string userPhone, string roleId, string oldRoleId, bool isEnable)
        {
            #region variables
            bool isEmailSent = false;
            bool isUserUpdated = false;
            EmailHelper helper;
            bool IsComplete = false;
            string htmlmessage = null;
            string callbackUrl = null;
            string loginUrl = baseUrlIdentityServer + "/ui/login";
            string Token = Guid.NewGuid().ToString();
            DateTime ExpiryDate = DateTime.Now.AddDays(1);
            string UserName = null;
            #endregion

            // Case 1: Exsisting User 
            if (!string.IsNullOrEmpty(userId) && !userId.Contains("null"))
            {
                isUserUpdated = _identityRepository.UpdateUser(userId, userPhone, oldRoleId, roleId, name, BoundaryConstants.BoundaryId, isEnable);
                return isUserUpdated;
            }

            // Case 2: New User
            else
            {
                bool isUserCreated = _identityRepository.CreateUser(userEmail, name, userPhone, roleId, BoundaryConstants.BoundaryId, isEnable);
                string redirectUri =  GenericHelper.EncodeUrl(GetRedirectUrl(roleId)); 
                if (isUserCreated)
                {
                    #region Send Email to User to activate account 
                    UserName = _identityRepository.GetUserNameByEmail(userEmail.Trim());
                    callbackUrl = string.Format("{0}/ui/ResetPassword?TokenKey={1}&redirectUri={2}", baseUrlIdentityServer,Token, redirectUri);

                    if (_identityRepository.SaveToken(userEmail.Trim(), Token, ExpiryDate, IsComplete))
                    {
                        htmlmessage = String.Format("<b>Hi {0}.</b><br/><br/>You have been invited to the BitzerIoc system." +
                                      "<br/><br/>User name = {0} <br/><br/>" +
                                      "To activate your account and create a password please <a href='{1}' > click here. </a>" +
                                      "<br/><br/>You can access the Remote Caretaking system <a href='{2}'> here. </a> <br/><br/>Best regards",
                                      UserName, callbackUrl, loginUrl);


                        helper = new EmailHelper(userEmail.Trim(), EmailConstants.From, "Setup password request", "", htmlmessage, null);

                        isEmailSent = helper.SendEmailAsync().Result;


                    }
                    #endregion
                }
                return isEmailSent;
            }

        }
        #endregion


        #region Helper Method
        /// <summary>
        /// Get the redirected url base on the role of user.
        /// If role of user is admin then redirect url of admin site
        /// If role of user is User than redirect url of user site
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private string GetRedirectUrl(string roleId)
        {
            string roleName = _identityRepository.GetRoles().Where(r => r.RoleId.Equals(roleId, StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Name;
            switch (roleName)
            {
                case RoleConstants.UserRole:
                    return baseUrlBitzerIoC;
                case RoleConstants.AdminRole:
                    return baseUrlBitzerIoCAdmin;
                default:
                    return null;

            }
        }

        #endregion
    }
}
