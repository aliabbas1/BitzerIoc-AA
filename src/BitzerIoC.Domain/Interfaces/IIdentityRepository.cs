using BitzerIoC.Domain.DTO;
using BitzerIoC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Interfaces
{
    /// <summary>
    /// Interface for Identity Repository
    /// </summary>
    public interface IIdentityRepository
    {
        IEnumerable<AspNetUser> GetUsers();
        IEnumerable<AspNetUserClaim> GetUserClaims();
        IEnumerable<AspNetUserClaim> GetUserClaims(string userId);
        bool ValidateEmail(string email);
        string GetUserNameByEmail(string email);
        AspNetUser GetUserById(string userId);
        bool SaveToken(string userName, string tokenKey, DateTime expiryDate, bool isCompleted = false);
        bool ValidateToken(string token);
        bool SavePassword(string tokenKey, string hashedPassword, string securehasSalt, bool isEnabled = true);
        AspNetUser GetValidUser(string userName);
        IEnumerable<UserDetailDTO> GetUsersWithRoles(int boundaryId);
        IEnumerable<AspNetRolesDTO> GetRolesDTO();
        IEnumerable<AspNetRole> GetRoles();
        List<UserBoundary> GetUserBoundaries(string userName, string userId = null);
        AspNetUser GetUserByUsername(string userName);

        UserDetailDTO GetUserProfileByUsername(string userName, int boundaryId);
        bool CreateUser(string userName, string name, string phoneNumber, string roleId, int boundaryId, bool isEnable);
        bool UpdateUser(string userId, string userPhone, string oldRoleId, string newRoleId, string name, int boundaryId, bool isEnable);
        AspNetUser GetUserWithRole(string userId);
        UserBoundary GetUserBoundary(string userId, int boundaryId);
        AspNetUser GetUser(string username, bool isEnabled);
        AspNetUser GetUser(string username);
        List<Boundary> GetBoundaries();
        List<Gateway> GetGateways();
        bool DeleteUser(string userId);
        List<UserDevice> GetUserDevices(string userId, string username = null);
        AspNetUser GetUserWithChildEntities(string userId);
        bool UpdatePassword(string userName, string hashedPassword, string secureHashSalt);
        bool UpdateName(string userName, string name);
        bool ValidatePassword(string username, string plainTextPassword);
        ApplicationClient GetClient(string clientId);

    }
}
