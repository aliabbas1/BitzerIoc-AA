using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using BitzerIoC.Domain.DTO;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.AppConstants;
using BitzerIoC.Infrastructure.Utilities;
using BitzerIoC.Domain.Interfaces;
using BitzerIoC.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;

namespace BitzerIoC.Infrastructure.Repositories
{
    //Include,IncludeThen => http://stackoverflow.com/questions/36601227/include-collection-in-entity-framework-core

    public class IdentityRepository : IIdentityRepository
    {
        /// <summary>
        /// Debug Filtering
        /// </summary>
        public static string TAG = typeof(IdentityRepository).Name;
        private IdentityContext _db;
        private IPasswordHasher<AspNetUser> _passwordHasher;

        public IdentityRepository(IdentityContext identityContext)
        {
            _db = identityContext;
            _passwordHasher = new PasswordHasher<AspNetUser>();
        }

        /// <summary>
        /// Get collection of users
        /// </summary>
        /// <returns>Collection</returns>
        public IEnumerable<AspNetUser> GetUsers()
        {
            var users = _db.AspNetUsers.ToList();
            return users;
        }

        /// <summary>
        /// Get the collection of user claims
        /// </summary>
        /// <returns>Collection</returns>
        public IEnumerable<AspNetUserClaim> GetUserClaims()
        {
            var claims = _db.AspNetUserClaims.ToList();
            return claims;
        }

        /// <summary>
        /// Get the collection of claims of a user.
        /// </summary>
        /// <param name="userId">SubjectId, User id can be obtained from claims as sub -> SubjectId</param>
        /// <returns>Collection</returns>
        public IEnumerable<AspNetUserClaim> GetUserClaims(string userId)
        {
            var userClaims = _db.AspNetUserClaims.Where(c => c.UserId == userId).ToList();
            return userClaims;
        }


        /// <summary>
        ///  Function take email as paramter then validate the email address and return true if success.
        /// </summary>
        /// <param name="email">Email address of user</param>
        /// <returns>return true if valid email is provided.</returns>
        public bool ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var userEmail = _db.AspNetUsers.Where(u =>
                                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (userEmail != null)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  Function take user email address as parameter and return Username
        /// </summary>
        /// <returns>Username of user or null if no username found</returns>
        public string GetUserNameByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var username = _db.AspNetUsers.Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).Select(u => u.Name).FirstOrDefault();
                return username;
            }
            return null;
        }


        /// <summary>
        /// Save the token granted to user with Token Key and Expiry Date
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="tokenKey">Token key</param>
        /// <param name="expiryDate">Expiry Date of Token</param>
        /// <param name="isCompleted">Optional = false</param>
        /// <returns>If successfully saved return true</returns>
        public bool SaveToken(string userName, string tokenKey, DateTime expiryDate, bool isCompleted = false)
        {
            if (!string.IsNullOrEmpty(tokenKey))
            {
                try
                {
                    AspNetUser user = _db.AspNetUsers.SingleOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
                    user.TokenKey = tokenKey;
                    user.ExpiryDate = expiryDate;
                    user.IsComplete = isCompleted;
                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(TAG, ex);
                    throw ex;
                }
            }
            return false;
        }


        /// <summary>
        ///  Valid Token in order to update password version 1.0
        /// </summary>
        /// <returns>Collection</returns>
        public bool ValidateToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                AspNetUser user = _db.AspNetUsers
                                .FirstOrDefault(u => u.TokenKey.Equals(token) && u.IsEnable == true &&
                                (u.IsComplete == null || u.IsComplete == false) && u.ExpiryDate >= DateTime.Now);
                if (user != null)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// ToDo: Pending Testing
        ///  Save the hashed password of user along with secureHashSalt,
        ///  Get the token and verify status of token,
        ///  Throw exception if more than one record found
        ///  Version 2.0
        /// </summary>
        /// <param name="tokenKey">Token Key</param>
        /// <param name="hashedPassword">Hash Password</param>
        /// <param name="securehasSalt">Secure Hash Salt</param>
        /// <param name="isEnabled">Optional [default:true]</param>
        /// <returns></returns>
        public bool SavePassword(string tokenKey, string hashedPassword, string secureHashSalt, bool isEnabled = true)
        {
            if (!string.IsNullOrEmpty(tokenKey))
            {
                try
                {
                    AspNetUser user = _db.AspNetUsers
                                      .Where(u => u.TokenKey.Equals(tokenKey)
                                       && u.IsEnable == isEnabled).SingleOrDefault();

                    if (user != null)
                    {
                        user.HashSalt = secureHashSalt;
                        user.Password = hashedPassword;
                        user.IsComplete = isEnabled;
                    }

                    _db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(TAG, ex);
                    throw ex;
                }
            }

            return false;
        }

        /// <summary>
        /// Validate that a user exist and valid user,
        /// return AspNetUser object or null
        /// </summary>
        /// <returns></returns>
        public AspNetUser GetValidUser(string userName)
        {
            return _db.AspNetUsers.FirstOrDefault((u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)));
        }



        /// <summary>
        /// This function returns DTo object which contains some information of user,
        /// if you need any more information then add properties in DTO and populate in linq statment below,
        /// Name,Username,Email,PhoneNumber,IsEnabled,RoleName collections as Roles is included in DTO
        /// </summary>
        /// <param name="boundryId"></param>
        /// <returns>IEnumerable of UserDetailDTO</returns>
        public IEnumerable<UserDetailDTO> GetUsersWithRoles(int boundaryId)
        {

            try
            {
                var users = (from user in _db.AspNetUsers.Include(u => u.AspNetUserRoles).ThenInclude(r => r.Role)
                             where user.AspNetUserRoles.Any(ub => ub.UserBoundary.BoundaryId == boundaryId)
                             orderby user.Name
                             select new UserDetailDTO
                             {
                                 UserId = user.UserId,
                                 Name = user.Name,
                                 UserName = user.UserName,
                                 Email = user.Email,
                                 PhoneNumber = user.PhoneNumber,
                                 IsEnable = user.IsEnable,
                                 //Dynamic                             
                                 Roles = user.AspNetUserRoles.Select(u => u.Role.Name).ToList(),
                                 RoleId = user.AspNetUserRoles.Select(u => u.Role.RoleId).ToList()
                             });
                return users;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(TAG, ex);
                throw ex;
            }
        }


        /// <summary>
        /// Get the roles in AspNetRole Table (DTO)
        /// </summary>
        /// <returns>Collection of AspNetRolesDTO</returns>
        public IEnumerable<AspNetRolesDTO> GetRolesDTO()
        {
            var roles = from role in _db.AspNetRoles
                        orderby role.RoleId descending
                        select new AspNetRolesDTO
                        { RoleId = role.RoleId, RoleName = role.Name };
            return roles.ToList();
        }

        /// <summary>
        /// Get the roles in AspNetRole Table )
        /// </summary>
        /// <returns>Collection of AspNetRoles</returns>
        public IEnumerable<AspNetRole> GetRoles()
        {
            var roles = from role in _db.AspNetRoles
                        orderby role.RoleId descending
                        select role;
            return roles.ToList();
        }


        /// <summary>
        /// Get the boundaries of a particular user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Collection of boundaries of a user else return null</returns>
        public List<UserBoundary> GetUserBoundaries(string userName, string userId = null)
        {
            List<UserBoundary> userBoundaries = null;
            AspNetUser user = null;

            if (userId != null)
                user = GetUserById(userId);
            else
                user = GetUserByUsername(userName);

            if (user != null)
            {
                userBoundaries = (from ub in _db.UserBoundaries
                                  where ub.UserId.Equals(user.UserId)
                                  select ub).ToList();
            }
            return userBoundaries;
        }



        /// <summary>
        /// Get the User object by username,
        /// Throw Exception if more than one user found
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>AspNetUsers object or null</returns>
        public AspNetUser GetUserByUsername(string userName)
        {
            return _db.AspNetUsers.SingleOrDefault(user => user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get the User object by userId,
        /// Throw Exception if more than one user found
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>AspNetUsers object or null</returns>
        public AspNetUser GetUserById(string userId)
        {
            return _db.AspNetUsers.SingleOrDefault(user => user.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Take username and boundaryId as parameter and return UserDetailDTO object,
        /// Throw exception if more then one record found.
        /// </summary>
        ///<param name="userName">username</param>
        /// <param name="boundaryId">boundary id</param>
        /// <returns></returns>
        public UserDetailDTO GetUserProfileByUsername(string userName, int boundaryId)
        {
            UserDetailDTO userDetail = (from user in _db.AspNetUsers.Include(u => u.AspNetUserRoles).ThenInclude(r => r.Role)
                                        where user.AspNetUserRoles.Any(b => b.UserBoundary.BoundaryId == boundaryId)
                                              && user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                                        select new UserDetailDTO
                                        {
                                            UserId = user.UserId,
                                            Name = user.Name,
                                            UserName = user.UserName,
                                            Email = user.Email,
                                            PhoneNumber = user.PhoneNumber,
                                            IsEnable = user.IsEnable,
                                            Password = user.Password,
                                            //Dynamic                             
                                            Roles = user.AspNetUserRoles.Select(u => u.Role.Name).ToList(),
                                            RoleId = user.AspNetUserRoles.Select(u => u.Role.RoleId).ToList()
                                        }).SingleOrDefault();
            return userDetail;
        }

        /// <summary>
        /// Take Id and boundaryId as parameter and return UserDetailDTO object,
        /// Throw exception if more then one record found.
        /// </summary>
        ///<param name="Id">Id</param>
        /// <param name="boundaryId">boundary id</param>
        /// <returns></returns>
        public UserDetailDTO GetUserProfileById(string userId, int boundaryId)
        {
            UserDetailDTO userDetail = (from user in _db.AspNetUsers.Include(u => u.AspNetUserRoles).ThenInclude(r => r.Role)
                                        where user.AspNetUserRoles.Any(b => b.UserBoundary.BoundaryId == boundaryId)
                                              && user.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                                        select new UserDetailDTO
                                        {
                                            UserId = user.UserId,
                                            Name = user.Name,
                                            UserName = user.UserName,
                                            Email = user.Email,
                                            PhoneNumber = user.PhoneNumber,
                                            IsEnable = user.IsEnable,
                                            Password = user.Password,
                                            //Dynamic                             
                                            Roles = user.AspNetUserRoles.Select(u => u.Role.Name).ToList(),
                                            RoleId = user.AspNetUserRoles.Select(u => u.Role.RoleId).ToList()
                                        }).SingleOrDefault();
            return userDetail;
        }

        /// <summary>
        /// ToDo: Test Pending
        /// Validate user password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="plainTextPassword"></param>
        /// <returns></returns>
        public bool ValidatePassword(string username, string plainTextPassword)
        {

            var user = GetUserByUsername(username);
            if (user == null || user.HashSalt == null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, plainTextPassword + user.HashSalt);

            switch (result)
            {
                case PasswordVerificationResult.Success:
                    return true;
                case PasswordVerificationResult.Failed:
                    return false;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create New User, return true if user is successfully created
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="roleId">Role assigned to user</param>
        /// <param name="boundaryId">Boundary Id</param>
        /// <param name="isEnable"></param>
        /// <returns>bool</returns>
        public bool CreateUser(string userName, string name, string phoneNumber, string roleId, int boundaryId, bool isEnable)
        {
            UserBoundary ub = new UserBoundary();
            Boundary boundary = GetBoundaries().SingleOrDefault(b => b.BoundaryId == boundaryId);
            AspNetRole role = GetRoles().SingleOrDefault(r => r.RoleId.Equals(roleId, StringComparison.OrdinalIgnoreCase));
            #region Boundary/Role Validation
            if (boundary == null || role == null)
            {
                return false;
            }
            #endregion


            try
            {
                AspNetUser user = new AspNetUser()
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = userName,
                    Name = name,
                    Email = userName,
                    Password = "-NO-PASSWORD-" + Guid.NewGuid(),
                    PhoneNumber = phoneNumber,
                    IsEnable = isEnable,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ExpiryDate = DateTime.Now,
                    StatusId = StatusConstants.Active
                };

                #region User boundaries
                ub.Boundary = boundary;
                ub.BoundaryId = boundaryId;
                ub.UserId = user.UserId;
                user.UserBoundaries = new List<UserBoundary> { ub };
                #endregion

                #region User Claims
                List<AspNetUserClaim> userClaims = new List<AspNetUserClaim>()
                {
                     new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Subject",
                         ClaimValue =user.UserId
                    },
                    new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Name",
                         ClaimValue =user.Name
                    },
                    new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Email",
                         ClaimValue =user.Email
                    },
                    new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="PhoneNumber",
                         ClaimValue =user.PhoneNumber
                    },
                    new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Role",
                         ClaimValue =role.RoleId
                    },
                    new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Guest",
                         ClaimValue = "Guest"
                    },
                                new AspNetUserClaim()
                    {
                         User = user,
                         UserId = user.UserId,
                         ClaimType="Installer",
                         ClaimValue = "Installer"
                    }


                };
                user.UserClaims = userClaims;
                #endregion

                using (_db.Database.BeginTransaction())
                {
                    user.AspNetUserRoles = new List<AspNetUserRole>();
                    user.AspNetUserRoles.Add(new AspNetUserRole() { Role = role, RoleId = role.RoleId, UserBoundary = ub, UserBoundaryId = ub.UserBoundaryId });

                    _db.AspNetUsers.Add(user);
                    _db.SaveChanges();
                    _db.Database.CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _db.Database.RollbackTransaction();
                LogHelper.WriteLog(TAG, ex);
                throw ex;
            }

        }





        /// <summary>
        /// Update user information liek name,phone,role Version 2.0
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="userPhone">Phone Number</param>
        /// <param name="oldRoleId">Old Role Id</param>
        /// <param name="newRoleId">New Role Id</param>
        /// <param name="name">Name of User</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <param name="isEnable">Enable/Disabled</param>
        /// <returns></returns>
        public bool UpdateUser(string userId, string userPhone, string oldRoleId, string newRoleId, string name, int boundaryId, bool isEnable)
        {
            //ToDo :: Pending test
            AspNetUser user = GetUserWithRole(userId);
            AspNetRole newRole = GetRoles().SingleOrDefault(r => r.RoleId.Equals(newRoleId, StringComparison.OrdinalIgnoreCase));
            UserBoundary userBoundary = GetUserBoundary(userId, boundaryId);

            #region Validation
            if (user == null || newRole == null || userBoundary == null)
            {
                return false;
            }
            #endregion

            try
            {
                _db.Database.BeginTransaction();
                user.PhoneNumber = userPhone;
                user.IsEnable = isEnable;
                user.Name = name;

                UpdateUserRole(userId, oldRoleId, user, newRole, userBoundary);

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _db.Database.RollbackTransaction();
                LogHelper.WriteLog(TAG, ex);
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Update User role information,if new and old roleid are same then do nothing
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldRoleId"></param>
        /// <param name="user"></param>
        /// <param name="newRole"></param>
        /// <param name="userBoundary"></param>
        private static void UpdateUserRole(string userId, string oldRoleId, AspNetUser user, AspNetRole newRole, UserBoundary userBoundary)
        {
            if (oldRoleId.Equals(newRole.RoleId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            //remove the role first
            AspNetUserRole userAssignedRole = user.AspNetUserRoles.SingleOrDefault(ur => ur.RoleId.Equals(oldRoleId, StringComparison.OrdinalIgnoreCase)
                                               && ur.UserBoundaryId == userBoundary.UserBoundaryId
                                               && ur.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));

            if (userAssignedRole != null)
            //remove the role assigned to user
            {
                user.AspNetUserRoles.Remove(userAssignedRole);
            }


            //Assign new role
            user.AspNetUserRoles.Add(new AspNetUserRole
            {
                UserId = user.UserId,
                User = user,
                RoleId = newRole.RoleId,
                Role = newRole,
                UserBoundaryId = userBoundary.UserBoundaryId,
                UserBoundary = userBoundary
            });
        }

        /// <summary>
        /// Todo: Pending Testing
        /// Get the user with role information.
        /// Return AspNetUsers object
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>AspNetUsers oject</returns>
        public AspNetUser GetUserWithRole(string userId)
        {
            AspNetUser user = (from usr in _db.AspNetUsers.Include(u => u.AspNetUserRoles).ThenInclude(r => r.Role)
                               where usr.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                               select usr).SingleOrDefault();

            return user;
        }


        /// <summary>
        /// Get the UserBoundaries object of a user
        /// User should have unqiue record otherwise exception is thrown
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <returns>UserBoundaries object</returns>
        public UserBoundary GetUserBoundary(string userId, int boundaryId)
        {
            UserBoundary userBoundary = _db.UserBoundaries.Where(ub => ub.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                                                                            && ub.BoundaryId == boundaryId
                                                                            && ub.UserId != null).SingleOrDefault();
            return userBoundary;
        }


        /// <summary>
        /// Get the user which is enabled/disabled by user name and status, returns AspNetUsers object,
        /// Throw exception if more than one user found.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="isEnabled">true or false</param>
        /// <returns>AspNetUsers object</returns>
        public AspNetUser GetUser(string username, bool isEnabled)
        {
            return _db.AspNetUsers.Where(x => x.IsEnable == isEnabled).SingleOrDefault((x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }


        /// <summary>
        /// Get the user by username, returns AspNetUsers object,
        /// Throw exception if more than one user found.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>AspNetUsers object</returns>
        public AspNetUser GetUser(string username)
        {
            return _db.AspNetUsers.SingleOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        }

        /// <summary>
        /// Get the boundaries return List of Boundaries
        /// </summary>
        /// <returns>List of Boundaries</returns>
        public List<Boundary> GetBoundaries()
        {
            //ToDo: Test Pending
            return _db.Boundaries.ToList();
        }


        public List<Gateway> GetGateways()
        {
            return _db.Gateways.ToList();
        }


        /// <summary>
        /// Delete user devices,roles,claims
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>true if successful</returns>
        public bool DeleteUser(string userId)
        {
            AspNetUser user = this.GetUserWithChildEntities(userId);

            if (user != null)
            {
                _db.AspNetUsers.Remove(user);
                _db.SaveChanges();
                return true;
            }

            return false;
        }




        /// <summary>
        /// Delete user device, if success than return true
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="deviceId">Targeted device id</param>
        /// <returns></returns>
        public bool DeleteUserDevice(string userId, int deviceId)
        {
            UserDevice userDevice = (from ud in _db.UserDevices
                                     where ud.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                                           && ud.DeviceId == deviceId
                                     select ud)
                                     .SingleOrDefault();
            if (userDevice != null)
            {
                _db.UserDevices.Remove(userDevice);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete user device, if success than return true
        /// </summary>
        /// <param name="userDeviceId">UserDevice Id (Key)</param>
        /// <returns></returns>
        public bool DeleteUserDevice(int userDeviceId)
        {
            UserDevice userDevice = (from ud in _db.UserDevices
                                     where ud.UserDeviceId == userDeviceId
                                     select ud)
                                     .SingleOrDefault();
            if (userDevice != null)
            {
                _db.UserDevices.Remove(userDevice);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete the [Boundary] of user and [Role] associated with that boundary,
        /// if successfull then return true
        /// </summary>
        /// <param name="username">SubjectId/UserId</param>
        /// <param name="boundaryId">Targeted boundary id</param>
        /// <returns>true if success</returns>
        public bool DeleteUserBoundary(string userId, int boundaryId)
        {
            UserBoundary userBoundary = (from ub in _db.UserBoundaries
                                         where ub.BoundaryId == boundaryId
                                               && ub.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                                         select ub).Include(ub => ub.AspNetUserRoles)
                                        .SingleOrDefault();

            if (userBoundary != null)
            {
                _db.UserBoundaries.Remove(userBoundary);
                _db.SaveChanges();
                return true;
            }

            return false;
        }


        /// <summary>
        /// Get the devices of a user, List of user devices
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="username">optional (default=null)</param>
        /// <returns>List of user devices</returns>
        public List<UserDevice> GetUserDevices(string userId, string username = null)
        {
            return (from ud in _db.UserDevices
                    where ud.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                    select ud).ToList();
        }

        /// <summary>
        /// Get user with child records.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AspNetUser GetUserWithChildEntities(string userId)
        {
            AspNetUser user = _db.AspNetUsers.Include(uc => uc.UserClaims)
                                            .Include(u => u.AspNetUserRoles)
                                            .Include(ud => ud.UserDevices)
                                            .Include(ub => ub.UserBoundaries).
                                             SingleOrDefault(u => u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
            return user;

        }



        /// <summary>
        /// ToDo: Pending Test
        ///  Update password of user, Get username and check the user exist
        ///  Throw exception if more than one record found
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="secureHashSalt"></param>
        /// <returns></returns>
        public bool UpdatePassword(string userName, string hashedPassword, string secureHashSalt)
        {
            if (!string.IsNullOrEmpty(hashedPassword))
            {
                try
                {
                    AspNetUser user = _db.AspNetUsers
                                      .Where(u => u.UserName.Equals(userName)).SingleOrDefault();

                    if (user != null)
                    {
                        user.Password = hashedPassword;
                        user.HashSalt = secureHashSalt;
                    }

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(TAG, ex);
                    throw ex;
                }
            }

            return false;
        }



        /// <summary>
        ///  Update name of user, Get username and check the user exist
        ///  Throw exception if more than one record found
        /// </summary>
        /// <param name="userName">user Name</param>
        /// <param name="name">updated name of user</param>
        /// <returns></returns>
        public bool UpdateName(string userName, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    AspNetUser user = _db.AspNetUsers
                                      .Where(u => u.UserName.Equals(userName)).SingleOrDefault();

                    if (user != null)
                    {
                        user.Name = name;
                    }

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(TAG, ex);
                    throw ex;
                }
            }

            return false;
        }


        /// <summary>
        /// Get a specific application client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ApplicationClient GetClient(string clientId)
        {
            throw new NotImplementedException();

        }


    }
}
