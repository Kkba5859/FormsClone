using FormsClone.CSharp.UserManagement.Registration.Models;

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> HandleRegistration(RegistrationModel registerModel);
        Task SendRegistrationConfirmationAsync(string email);  
        Task<ValidationResult> ValidateRegistrationData(RegistrationModel registerModel);  
        Task<RegistrationResult> RegisterUserAsync(RegistrationModel model, bool isAdmin );
        Task<RegistrationResult> RegisterAdminAsync(RegistrationModel model);
    }
}