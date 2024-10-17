using FormsClone.Models.Registration.Model;
using FormsClone.Models.User.Service;

namespace FormsClone.Models.Registration.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;

        public RegistrationService(IUserService userService)
        {
            _userService = userService;
        }

        // Метод для регистрации пользователя
        public async Task<RegistrationResult> HandleRegistration(RegisterModel registerModel)
        {
            RegistrationResult registrationResult;

            // Валидация данных
            var validationResult = await ValidateRegistrationData(registerModel);
            if (!validationResult.IsValid)
            {
                return new RegistrationResult(false, validationResult.Errors);
            }

            if (await UserExistsAsync(registerModel.Email))
            {
                return new RegistrationResult(false, new[] { "Пользователь с таким email уже существует." });
            }

            if (registerModel.IsAdmin)
            {
                // Регистрация как администратор
                registrationResult = await _userService.RegisterAdminAsync(registerModel);
            }
            else
            {
                // Регистрация как обычный пользователь
                registrationResult = await _userService.RegisterUserAsync(registerModel);
            }

            if (registrationResult.Succeeded)
            {
                // Отправка подтверждения регистрации
                await SendRegistrationConfirmationAsync(registerModel.Email);
                Console.WriteLine($"Пользователь {registerModel.Username} успешно зарегистрирован.");
                return new RegistrationResult(true);
            }

            // Обработка ошибок регистрации
            foreach (var error in registrationResult.Errors)
            {
                Console.WriteLine($"Ошибка: {error}");
            }
            return new RegistrationResult(false, registrationResult.Errors);
        }

        // Проверка на существование пользователя с таким email
        public async Task<bool> UserExistsAsync(string email)
        {
            var users = await _userService.GetUsersAsync();
            return users.Any(user => user.Email == email);
        }

        // Отправка подтверждения регистрации
        public async Task SendRegistrationConfirmationAsync(string email)
        {
            // Логика для отправки подтверждения, например, по email
            Console.WriteLine($"Отправлено письмо с подтверждением регистрации на {email}.");
            await Task.CompletedTask; // Эмуляция асинхронной операции
        }

        // Валидация данных регистрации
        public async Task<ValidationResult> ValidateRegistrationData(RegisterModel registerModel)
        {
            var errors = new List<string>();

            // Пример простой валидации
            if (string.IsNullOrWhiteSpace(registerModel.Username))
            {
                errors.Add("Имя пользователя не может быть пустым.");
            }
            if (string.IsNullOrWhiteSpace(registerModel.Email))
            {
                errors.Add("Email не может быть пустым.");
            }
            if (string.IsNullOrWhiteSpace(registerModel.Password))
            {
                errors.Add("Пароль не может быть пустым.");
            }

            // Возвращаем результат валидации
            return await Task.FromResult(new ValidationResult(errors.Count == 0, errors));
        }
    }
}
