using Blazored.LocalStorage;
using Blazored.Modal;
using FormsClone.Data;
using FormsClone.Models.Registration.Service;
using FormsClone.Models.User.Service;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

namespace FormsClone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Используем SQLite для локального хранения данных
            var connectionString = "Data Source=FormsClone.db"; // Проверьте, что путь корректен
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddSingleton<AuthStateService>();

            // Добавление LocalStorage для локального хранения пользователей
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddBlazoredModal();

            builder.Services.AddScoped<ITemplatesService, TemplatesService>();

            // Регистрация объединённого сервиса для работы с пользователями (и администраторами)
            builder.Services.AddScoped<IUserService, UserService>();


            // Регистрация модели для работы с регистрацией
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            await builder.Build().RunAsync();
        }
    }
}
