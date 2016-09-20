using System.Threading.Tasks;
using IdentityServer4.Validation;
using BitzerIoC.Domain.Interfaces;

namespace Host.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityRepository _repository;

        public ResourceOwnerPasswordValidator(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public Task<CustomGrantValidationResult> ValidateAsync(string userName, string password, ValidatedTokenRequest request)
        {
            if (_repository.ValidatePassword(userName, password))
            {
                return Task.FromResult(new CustomGrantValidationResult(userName, "password"));
            }
       
            return Task.FromResult(new CustomGrantValidationResult("Wrong username or password"));
        }
    }
}
