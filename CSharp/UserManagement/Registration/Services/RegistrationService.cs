using Blazored.LocalStorage; // Подключение для работы с локальным хранилищем.
using FormsClone.CSharp.UserManagement.AdminDashboard.Models; // Подключение модели пользователя.
using FormsClone.CSharp.UserManagement.AdminDashboard.Services; // Подключение сервисов для работы с пользователями.
using FormsClone.CSharp.UserManagement.Interfaces; // Подключение интерфейсов.
using FormsClone.CSharp.UserManagement.Registration.Models; // Подключение моделей регистрации.
using Microsoft.AspNetCore.Components; // Подключение для работы с компонентами.
using Microsoft.JSInterop; // Подключение для работы с JavaScript.

namespace FormsClone.CSharp.UserManagement.Registration.Services
{
    public class RegistrationService : IRegistrationService // Реализация интерфейса IRegistrationService.
    {
        private readonly ILocalStorageService _localStorage; // Сервис для работы с локальным хранилищем.
        private readonly IUserService _userService; // Сервис для работы с пользователями.

        public RegistrationService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, IUserService userService, AuthStateService authStateService)
        {
            _localStorage = localStorage; // Инициализация сервиса локального хранилища.
            _userService = userService; // Инициализация сервиса пользователей.
        }

        public async Task<RegistrationResult> HandleRegistration(RegistrationModel registerModel) // Метод обработки регистрации.
        {
            RegistrationResult registrationResult; // Переменная для хранения результата регистрации.

            var validationResult = await ValidateRegistrationData(registerModel); // Валидация данных регистрации.
            if (!validationResult.IsValid) // Проверка на валидность.
            {
                return new RegistrationResult(false, validationResult.Errors); // Возврат результата с ошибками.
            }

            // Регистрация пользователя или администратора в зависимости от флага.
            registrationResult = registerModel.IsAdmin
                ? await RegisterAdminAsync(registerModel)
                : await RegisterUserAsync(registerModel);

            if (registrationResult.Succeeded) // Проверка на успешность регистрации.
            {
                await SendRegistrationConfirmationAsync(registerModel.Email); // Отправка письма с подтверждением регистрации.
                Console.WriteLine($"Пользователь {registerModel.Username} успешно зарегистрирован.");
                return new RegistrationResult(true); // Возврат успешного результата.
            }

            // Вывод ошибок, если регистрация не удалась.
            foreach (var error in registrationResult.Errors)
            {
                Console.WriteLine($"Ошибка: {error}");
            }

            return new RegistrationResult(false, registrationResult.Errors); // Возврат результата с ошибками.
        }

        public async Task SendRegistrationConfirmationAsync(string email) // Метод отправки подтверждения регистрации.
        {
            Console.WriteLine($"Отправлено письмо с подтверждением регистрации на {email}."); // Вывод сообщения о подтверждении.
            await Task.CompletedTask; // Завершение задачи.
        }

        public async Task<ValidationResult> ValidateRegistrationData(RegistrationModel registerModel) // Метод валидации данных регистрации.
        {
            var errors = new List<string>(); // Список ошибок.

            // Проверка на пустые поля.
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

            return await Task.FromResult(new ValidationResult(errors.Count == 0, errors)); // Возврат результата валидации.
        }

        public async Task<RegistrationResult> RegisterUserAsync(RegistrationModel? model, bool isAdmin = false) // Метод регистрации пользователя.
        {
            if (model == null) // Проверка на null.
                return new RegistrationResult(false, new List<string> { "Модель регистрации не может быть пустой." });

            // Проверка на пустые поля.
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return new RegistrationResult(false, new List<string> { "Имя пользователя, Email и Пароль не могут быть пустыми." });

            try
            {
                // Проверка на существование пользователя.
                var existingUser = await _localStorage.GetItemAsync<UserModel>($"user_{model.Username}");
                if (existingUser != null)
                    return new RegistrationResult(false, new List<string> { "Пользователь уже существует." });

                // Создание нового пользователя и сохранение в локальное хранилище.
                var user = new UserModel(username: model.Username, password: model.Password, isAdmin: isAdmin);
                await _localStorage.SetItemAsync($"user_{model.Username}", user);

                return new RegistrationResult(true); // Возврат успешного результата.
            }
            catch (Exception ex) // Обработка ошибок.
            {
                Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                return new RegistrationResult(false, new List<string> { "Ошибка при регистрации пользователя." }); // Возврат результата с ошибкой.
            }
        }

        public async Task<RegistrationResult> RegisterAdminAsync(RegistrationModel model) // Метод регистрации администратора.
        {
            if (model == null) // Проверка на null.
                return new RegistrationResult(false, new List<string> { "Модель регистрации не может быть пустой." });

            // Создание нового администратора и сохранение в локальное хранилище.
            var user = new UserModel(username: model.Username, password: model.Password, isAdmin: true);
            await _localStorage.SetItemAsync($"user_{model.Username}", user);

            return new RegistrationResult(true); // Возврат успешного результата.
        }
    }
}
