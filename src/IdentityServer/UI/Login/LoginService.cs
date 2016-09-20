using IdentityServer4.Services.InMemory;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.Security;
using BitzerIoC.Domain.Entities;
using Host.Configuration;
using BitzerIoC.Infrastructure.Utilities;
using IdentityServer4.Validation;
using BitzerIoC.Domain.Interfaces;

namespace Host.UI.Login
{
    public class LoginService
    {
        private readonly IResourceOwnerPasswordValidator _passwordValidator;
        private readonly IIdentityRepository _repository;

        public LoginService(IResourceOwnerPasswordValidator passwordValidator, IIdentityRepository repository)
        {
            _passwordValidator = passwordValidator;
            _repository = repository;
        }

        public bool ValidateCredentials(string username, string password)
        {
            return _repository.ValidatePassword(username, password);            
        }

        public AspNetUser FindByUsername(string username)
        {
            return _repository.GetUserByUsername(username);
        }

        #region Cusstomized
        /// <summary>
        /// Customized Method used to check user status
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool isUserEnabled(string username)
        {
            var EnableUser = _repository.GetUser(username);
            if (EnableUser != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validate that User have role ="User", return true if success
        /// </summary>
        /// <param name="memUser">In Memory User</param>
        /// <returns></returns>
        public bool validateUserRole(InMemoryUser memUser)
        {
            Claim roleClaim = memUser.Claims.Where(ct => ct.Type.Equals("role")).SingleOrDefault();
            if (roleClaim != null)
            {
                if (roleClaim.Value.Equals("User"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validate that User have role ="Admin", return true if success
        /// </summary>
        /// <param name="memUser">In Memory User</param>
        /// <returns></returns>
        public bool validateAdminRole(InMemoryUser memUser)
        {
            Claim roleClaim = memUser.Claims.Where(ct => ct.Type.Equals("role")).SingleOrDefault();
            if (roleClaim != null)
            {
                if (roleClaim.Value.Equals("Admin"))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Return the collection of Asp net User Claims
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<AspNetUserClaim> GetUserClaims(string userId)
        {
            return _repository.GetUserClaims(userId).ToList(); ;
        }
        #endregion

    }
}
