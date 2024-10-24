using Blazored.LocalStorage;
using FormsClone.CSharp.UserManagement.AdminDashboard.Models;
using FormsClone.CSharp.UserManagement.Interfaces;
using FormsClone.CSharp.UserManagement.Registration.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class UserService : IUserService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly AuthStateService _authStateService; 

        public UserService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, AuthStateService authStateService)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _authStateService = authStateService; 
        }

        // Регистрация
        public async Task<RegistrationResult> RegisterUserAsync(RegistrationModel? model, bool isAdmin = false)
        {
            if (model == null)
                return new RegistrationResult(false, new List<string> { "Registration model cannot be null." });

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return new RegistrationResult(false, new List<string> { "Username, Email, and Password must not be empty." });

            try
            {
                var existingUser = await _localStorage.GetItemAsync<UserModel>($"user_{model.Username}");
                if (existingUser != null)
                    return new RegistrationResult(false, new List<string> { "User already exists." });

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
            return await RegisterUserAsync(model, true);
        }


        public async Task LogoutUserAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync("currentUser");
                _authStateService.IsLoggedIn = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выходе пользователя: {ex.Message}");
            }
        }
        
        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = new List<UserModel>();
            try
            {
                foreach (var key in await _localStorage.KeysAsync())
                {
                    if (key == "currentUser" || !key.StartsWith("user_")) continue;
                    var user = await _localStorage.GetItemAsync<UserModel>(key);
                    if (user != null && !users.Exists(u => u.Username == user.Username))
                    {
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}");
            }
            return users;
        }

        public async Task BlockUserAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, true, isBlocked: true);
        }

        public async Task UnblockUserAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, false, isBlocked: false);
        }

        public async Task DeleteUserAsync(string userId)
        {
            try
            {
                await _localStorage.RemoveItemAsync($"user_{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
            }
        }

        public async Task AddToAdminAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, true, isAdmin: true);
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    user.IsAdmin = true;
                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя в администраторы: {ex.Message}");
            }
        }

        public async Task RemoveFromAdminAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, false, isAdmin: false);
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    user.IsAdmin = false;
                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя из администраторов: {ex.Message}");
            }
        }

        private async Task UpdateUserStatusAsync(string userId, bool value, bool isBlocked = false, bool isAdmin = false)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    if (isBlocked)
                        user.IsBlocked = value;
                    if (isAdmin)
                        user.IsAdmin = value;

                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении статуса пользователя: {ex.Message}");
            }
        }

        public async Task<string> GetUserNameAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user?.Username ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении имени пользователя: {ex.Message}");
                return string.Empty;
            }
        }

        public void NavigateToDashboard()
        {
            _navigationManager.NavigateTo("/dashboard");
        }
    }

    public class AuthStateService
    {
        public event Action? OnChange;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
