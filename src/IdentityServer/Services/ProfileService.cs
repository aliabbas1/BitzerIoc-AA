using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.Interfaces;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Host.Services
{
    public class ProfileService: IProfileService
    {
        private readonly IIdentityRepository _repository;

        public ProfileService(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();

            AspNetUser user = _repository.GetUserById(subjectId);
            user.UserClaims = _repository.GetUserClaims(user.UserId).ToList();

            List<Claim> claims = new List<Claim>();
          
            foreach (var claim in user.UserClaims)
            {
                claims.Add(new Claim(ReturnJwtClaimType(claim.ClaimType), claim.ClaimValue));
            }

            context.IssuedClaims = claims;

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _repository.GetUserById(context.Subject.GetSubjectId());

            context.IsActive = (user != null) && user.IsEnable;
            return Task.FromResult(0);
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
                case "Subject":
                    return JwtClaimTypes.Subject;
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
