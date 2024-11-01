using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FormsClone.CSharp.MainFunctionality.Interfaces;
using FormsClone.CSharp.MainFunctionality.Forms.Models;
using FormsClone.CSharp.MainFunctionality.Forms.Services;
using FormsClone.CSharp.MainFunctionality.Questions.Models;
using FormsClone.CSharp.MainFunctionality.Questions.Services;
using FormsClone.CSharp.MainFunctionality.Templates.Models;
using FormsClone.CSharp.UserManagement.Registration.Services;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using FormsClone.CSharp.UserManagement.Interfaces;
using FormsClone.CSharp.UserManagement.Login.Services;
using FormsClone.CSharp.HomePage;
using FormsClone.CSharp.MainFunctionality.Templates.Services;
using System.Net.Http; // Подключаем пространство имен для HttpClient

namespace FormsClone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Регистрация HttpClient
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://flow-ability-1520.my.salesforce.com/") });

            // Регистрируем сервис для работы с локальным хранилищем
            builder.Services.AddBlazoredLocalStorage();

            // Регистрируем сервисы авторизации и управления пользователями
            builder.Services.AddSingleton<AuthStateService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<ILoginService, LoginService>();

            // Регистрируем сервисы для работы с сущностями "Form", "Question" и "Template"
            builder.Services.AddScoped<IEntityService<Form>, FormsService>();
            builder.Services.AddScoped<IEntityService<Question>, QuestionsService>();
            builder.Services.AddScoped<IEntityService<Template>, TemplatesService>();

            // Регистрируем другие необходимые сервисы для работы приложения
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ITabService, TabService>();
            builder.Services.AddScoped<ISalesforceService, SalesforceService>();

            await builder.Build().RunAsync();
        }
    }
}
