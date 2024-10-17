using FormsClone.Models.Registration.Model;

namespace FormsClone.Models.Registration.Service
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> HandleRegistration(RegisterModel registerModel);  // Универсальный метод регистрации

        Task<bool> UserExistsAsync(string email);  // Проверка существования пользователя

        Task SendRegistrationConfirmationAsync(string email);  // Отправка подтверждения регистрации

        Task<ValidationResult> ValidateRegistrationData(RegisterModel registerModel);  // Валидация данных регистрации
    }
}