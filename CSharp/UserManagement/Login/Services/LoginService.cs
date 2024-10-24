using Blazored.LocalStorage;
using FormsClone.CSharp.UserManagement.AdminDashboard.Models;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using FormsClone.CSharp.UserManagement.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FormsClone.CSharp.UserManagement.Login.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly AuthStateService _authStateService; 

        public LoginService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, AuthStateService authStateService)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _authStateService = authStateService; 
        }
        public async Task<(bool Success, string Message, UserModel User)> LoginUserAsync(string? username, string? password)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{username}");
                if (user != null)
                {
                    if (user.IsBlocked)
                    {
                        return (false, "Извините, вы заблокированы! Для получения дополнительной информации отправьте электронное письмо на адрес: kkba5859@gmail.com", null);
                    }

                    if (user.Password == password)
                    {
                        await _localStorage.SetItemAsync("currentUser", user);
                        _authStateService.IsLoggedIn = true; 
                        return (true, string.Empty, user);
                    }

                    return (false, "Неверные данные для входа.", null);
                }

                return (false, "Пользователь не найден.", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе пользователя: {ex.Message}");
                return (false, "Произошла ошибка. Пожалуйста, попробуйте позже.", null);
            }
        }



        public async Task<bool> LoginAdminAsync(string? username, string? password)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{username}");
                if (user != null && user.Password == password && user.IsAdmin && !user.IsBlocked)
                {
                    await _localStorage.SetItemAsync("currentUser", user);
                    _authStateService.IsLoggedIn = true; 
                    NavigateToDashboard();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе администратора: {ex.Message}");
                return false;
            }
        }

        public void NavigateToDashboard()
        {
            _navigationManager.NavigateTo("/dashboard");
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                if (user == null)
                {
                    throw new InvalidOperationException("Текущий пользователь не найден."); 
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении текущего пользователя: {ex.Message}");
                throw; 
            }
        }


        public async Task SetLoggedInState(UserModel user)
        {
            try
            {
                await _localStorage.SetItemAsync("currentUser", user);
                _authStateService.IsLoggedIn = true; // Устанавливаем состояние аутентификации
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при установке состояния пользователя: {ex.Message}");
            }
        }
        public async Task<bool> IsUserLoggedInAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке состояния пользователя: {ex.Message}");
                return false;
            }
        }
        public async Task LogoutUserAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync("currentUser");
                _authStateService.IsLoggedIn = false; // Обновляем состояние аутентификации
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выходе пользователя: {ex.Message}");
            }
        }
        public async Task<bool> IsUserAdminAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user?.IsAdmin ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке прав администратора: {ex.Message}");
                return false;
            }
        }
    }

}
