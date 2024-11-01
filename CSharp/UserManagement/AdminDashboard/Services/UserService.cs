using Blazored.LocalStorage; // Подключение библиотеки для работы с локальным хранилищем.
using FormsClone.CSharp.UserManagement.AdminDashboard.Models; // Подключение модели UserModel.
using FormsClone.CSharp.UserManagement.Interfaces; // Подключение интерфейса IUserService.
using FormsClone.CSharp.UserManagement.Registration.Models; // Подключение модели RegistrationModel.
using Microsoft.AspNetCore.Components; // Подключение для работы с компонентами Blazor.
using Microsoft.JSInterop; // Подключение для работы с JavaScript в Blazor.

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class UserService : IUserService // Класс UserService реализует интерфейс IUserService.
    {
        private readonly ILocalStorageService _localStorage; // Сервис для работы с локальным хранилищем.
        private readonly NavigationManager _navigationManager; // Менеджер навигации для изменения маршрутов.
        private readonly AuthStateService _authStateService; // Сервис для отслеживания состояния аутентификации.

        public UserService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, AuthStateService authStateService) // Конструктор UserService.
        {
            _localStorage = localStorage; // Инициализация локального хранилища.
            _navigationManager = navigationManager; // Инициализация менеджера навигации.
            _authStateService = authStateService; // Инициализация сервиса состояния аутентификации.
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}"); // Получение пользователя по ID

                if (user == null) // Проверка, существует ли пользователь
                {
                    Console.WriteLine("Пользователь не найден."); // Логирование ошибки
                    return false; // Возврат false, если пользователь не найден
                }

                user.Password = newPassword; // Установка нового пароля
                await _localStorage.SetItemAsync($"user_{userId}", user); // Сохранение пользователя с обновленным паролем в локальном хранилище
                Console.WriteLine("Пароль успешно обновлен."); // Логирование успеха
                return true; // Возврат true, если пароль успешно изменен
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при изменении пароля: {ex.Message}"); // Логирование ошибки
                return false; // Возврат false в случае ошибки
            }
        }

        // Регистрация пользователя
        public async Task<RegistrationResult> RegisterUserAsync(RegistrationModel? model, bool isAdmin = false) // Асинхронный метод регистрации пользователя.
        {
            if (model == null) // Проверка на null для модели регистрации.
                return new RegistrationResult(false, new List<string> { "Registration model cannot be null." }); // Возврат ошибки, если модель null.

            // Проверка на пустые значения в модели
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return new RegistrationResult(false, new List<string> { "Username, Email, and Password must not be empty." }); // Возврат ошибки, если поля пустые.

            try
            {
                // Проверка существования пользователя в локальном хранилище
                var existingUser = await _localStorage.GetItemAsync<UserModel>($"user_{model.Username}");
                if (existingUser != null) // Если пользователь уже существует
                    return new RegistrationResult(false, new List<string> { "User already exists." }); // Возврат ошибки.

                // Создание нового пользователя
                var user = new UserModel(username: model.Username, password: model.Password, isAdmin: isAdmin);
                await _localStorage.SetItemAsync($"user_{model.Username}", user); // Сохранение пользователя в локальном хранилище.

                return new RegistrationResult(true); // Возврат успешного результата регистрации.
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}"); // Логирование ошибки.
                return new RegistrationResult(false, new List<string> { "Ошибка при регистрации пользователя." }); // Возврат ошибки.
            }
        }

        public async Task<RegistrationResult> RegisterAdminAsync(RegistrationModel model) // Метод регистрации администратора.
        {
            return await RegisterUserAsync(model, true); // Вызов метода регистрации пользователя с флагом isAdmin.
        }

        public async Task LogoutUserAsync() // Асинхронный метод выхода пользователя.
        {
            try
            {
                await _localStorage.RemoveItemAsync("currentUser"); // Удаление текущего пользователя из локального хранилища.
                _authStateService.IsLoggedIn = false; // Обновление состояния аутентификации.
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при выходе пользователя: {ex.Message}"); // Логирование ошибки.
            }
        }

        public async Task<List<UserModel>> GetUsersAsync() // Асинхронный метод получения списка пользователей.
        {
            var users = new List<UserModel>(); // Список для хранения пользователей.
            try
            {
                // Перебор ключей в локальном хранилище
                foreach (var key in await _localStorage.KeysAsync())
                {
                    if (key == "currentUser" || !key.StartsWith("user_")) continue; // Пропуск ключей текущего пользователя и некорректных ключей.
                    var user = await _localStorage.GetItemAsync<UserModel>(key); // Получение пользователя по ключу.
                    if (user != null && !users.Exists(u => u.Username == user.Username)) // Проверка на существование пользователя в списке
                    {
                        users.Add(user); // Добавление пользователя в список.
                    }
                }
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}"); // Логирование ошибки.
            }
            return users; // Возврат списка пользователей.
        }

        public async Task BlockUserAsync(string userId) // Асинхронный метод блокировки пользователя.
        {
            await UpdateUserStatusAsync(userId, true, isBlocked: true); // Вызов метода обновления статуса пользователя с блокировкой.
        }

        public async Task UnblockUserAsync(string userId) // Асинхронный метод разблокировки пользователя.
        {
            await UpdateUserStatusAsync(userId, false, isBlocked: false); // Вызов метода обновления статуса пользователя с разблокировкой.
        }

        public async Task DeleteUserAsync(string userId) // Асинхронный метод удаления пользователя.
        {
            try
            {
                await _localStorage.RemoveItemAsync($"user_{userId}"); // Удаление пользователя из локального хранилища.
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}"); // Логирование ошибки.
            }
        }

        public async Task AddToAdminAsync(string userId) // Асинхронный метод добавления пользователя в администраторы.
        {
            await UpdateUserStatusAsync(userId, true, isAdmin: true); // Вызов метода обновления статуса пользователя с добавлением в администраторы.
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}"); // Получение пользователя по ID.
                if (user != null) // Если пользователь найден
                {
                    user.IsAdmin = true; // Обновление статуса администратора.
                    await _localStorage.SetItemAsync($"user_{userId}", user); // Сохранение обновленного пользователя в локальном хранилище.
                }
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при добавлении пользователя в администраторы: {ex.Message}"); // Логирование ошибки.
            }
        }

        public async Task RemoveFromAdminAsync(string userId) // Асинхронный метод удаления пользователя из администраторов.
        {
            await UpdateUserStatusAsync(userId, false, isAdmin: false); // Вызов метода обновления статуса пользователя с удалением из администраторов.
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}"); // Получение пользователя по ID.
                if (user != null) // Если пользователь найден
                {
                    user.IsAdmin = false; // Обновление статуса администратора.
                    await _localStorage.SetItemAsync($"user_{userId}", user); // Сохранение обновленного пользователя в локальном хранилище.
                }
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при удалении пользователя из администраторов: {ex.Message}"); // Логирование ошибки.
            }
        }

        private async Task UpdateUserStatusAsync(string userId, bool value, bool isBlocked = false, bool isAdmin = false) // Приватный асинхронный метод обновления статуса пользователя.
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}"); // Получение пользователя по ID.
                if (user != null) // Если пользователь найден
                {
                    if (isBlocked) // Если нужно обновить статус блокировки
                        user.IsBlocked = value; // Обновление статуса блокировки.
                    if (isAdmin) // Если нужно обновить статус администратора
                        user.IsAdmin = value; // Обновление статуса администратора.

                    await _localStorage.SetItemAsync($"user_{userId}", user); // Сохранение обновленного пользователя в локальном хранилище.
                }
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при обновлении статуса пользователя: {ex.Message}"); // Логирование ошибки.
            }
        }

        public async Task<string> GetUserNameAsync() // Асинхронный метод получения имени текущего пользователя.
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser"); // Получение текущего пользователя из локального хранилища.
                return user?.Username ?? string.Empty; // Возврат имени пользователя или пустой строки.
            }
            catch (Exception ex) // Обработка исключений
            {
                Console.WriteLine($"Ошибка при получении имени пользователя: {ex.Message}"); // Логирование ошибки.
                return string.Empty; // Возврат пустой строки в случае ошибки.
            }
        }

        public void NavigateToDashboard() // Метод навигации на страницу панели управления.
        {
            _navigationManager.NavigateTo("/dashboard"); // Перенаправление на страницу панели управления.
        }
    }

    public class AuthStateService // Класс для отслеживания состояния аутентификации.
    {
        public event Action? OnChange; // Событие для уведомления об изменениях состояния.

        private bool _isLoggedIn; // Поле для хранения состояния аутентификации.
        public bool IsLoggedIn // Свойство для доступа к состоянию аутентификации.
        {
            get => _isLoggedIn; // Получение значения состояния.
            set // Установка значения состояния.
            {
                _isLoggedIn = value; // Обновление состояния.
                NotifyStateChanged(); // Уведомление об изменении состояния.
            }
        }



        private void NotifyStateChanged() => OnChange?.Invoke(); // Вызов события изменения состояния.
    }
}
