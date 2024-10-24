using Blazored.LocalStorage;
using FormsClone.CSharp.UserManagement.AdminDashboard.Models;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using FormsClone.CSharp.UserManagement.Interfaces;
using FormsClone.CSharp.UserManagement.Registration.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FormsClone.CSharp.UserManagement.Registration.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IUserService _userService; 

        public RegistrationService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, IUserService userService, AuthStateService authStateService)
        {
            _localStorage = localStorage;
            _userService = userService;
        }

        public async Task<RegistrationResult> HandleRegistration(RegistrationModel registerModel)
        {
            RegistrationResult registrationResult;

            var validationResult = await ValidateRegistrationData(registerModel);
            if (!validationResult.IsValid)
            {
                return new RegistrationResult(false, validationResult.Errors);
            }

            registrationResult = registerModel.IsAdmin
                ? await RegisterAdminAsync(registerModel)
                : await RegisterUserAsync(registerModel);

            if (registrationResult.Succeeded)
            {
                await SendRegistrationConfirmationAsync(registerModel.Email);
                Console.WriteLine($"Пользователь {registerModel.Username} успешно зарегистрирован.");
                return new RegistrationResult(true);
            }

            foreach (var error in registrationResult.Errors)
            {
                Console.WriteLine($"Ошибка: {error}");
            }

            return new RegistrationResult(false, registrationResult.Errors);
        }



        public async Task SendRegistrationConfirmationAsync(string email)
        {
            Console.WriteLine($"Отправлено письмо с подтверждением регистрации на {email}.");
            await Task.CompletedTask; 
        }

        public async Task<ValidationResult> ValidateRegistrationData(RegistrationModel registerModel)
        {
            var errors = new List<string>();

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

            return await Task.FromResult(new ValidationResult(errors.Count == 0, errors));
        }

        public async Task<RegistrationResult> RegisterUserAsync(RegistrationModel? model, bool isAdmin = false)
        {
            if (model == null)
                return new RegistrationResult(false, new List<string> { "Модель регистрации не может быть пустой." });

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return new RegistrationResult(false, new List<string> { "Имя пользователя, Email и Пароль не могут быть пустыми." });

            try
            {
                var existingUser = await _localStorage.GetItemAsync<UserModel>($"user_{model.Username}");
                if (existingUser != null)
                    return new RegistrationResult(false, new List<string> { "Пользователь уже существует." });

                var user = new UserModel(username: model.Username, password: model.Password, isAdmin: isAdmin);
                await _localStorage.SetItemAsync($"user_{model.Username}", user);

                return new RegistrationResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                return new RegistrationResult(false, new List<string> { "Ошибка при регистрации пользователя." });
            }
        }

        public async Task<RegistrationResult> RegisterAdminAsync(RegistrationModel model)
        {
            if (model == null)
                return new RegistrationResult(false, new List<string> { "Модель регистрации не может быть пустой." });

            var user = new UserModel(username: model.Username, password: model.Password, isAdmin: true);
            await _localStorage.SetItemAsync($"user_{model.Username}", user);

            return new RegistrationResult(true);
        }

    }
}
