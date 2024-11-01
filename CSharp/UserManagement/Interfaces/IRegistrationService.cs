using FormsClone.CSharp.UserManagement.Registration.Models; // Подключение модели RegistrationModel.

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IRegistrationService // Интерфейс IRegistrationService для управления процессом регистрации пользователей.
    {
        // Асинхронный метод для обработки регистрации.
        Task<RegistrationResult> HandleRegistration(RegistrationModel registerModel);

        // Асинхронный метод для отправки подтверждения регистрации на указанный email.
        Task SendRegistrationConfirmationAsync(string email);

        // Асинхронный метод для валидации данных регистрации.
        Task<ValidationResult> ValidateRegistrationData(RegistrationModel registerModel);

        // Асинхронный метод для регистрации пользователя.
        Task<RegistrationResult> RegisterUserAsync(RegistrationModel model, bool isAdmin);

        // Асинхронный метод для регистрации администратора.
        Task<RegistrationResult> RegisterAdminAsync(RegistrationModel model);
    }
}
