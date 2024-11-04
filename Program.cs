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
using System.Net.Http;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services.FormsClone.CSharp.UserManagement.AdminDashboard.Services;

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

            // Добавляем настройки для JiraService
            builder.Services.Configure<JiraSettings>(options =>
            {
                options.EncodedCredentials = "a2tiYTU4NTlAZ21haWwuY29tOkFUQVRUM3hGZkdGMHpjYW1JSktsYWVOTlhQQ3pxLWNIUWM4bm1SN3hGSDFVZENvRnl6RWxmUEI5eDA0UDc3WlppWTdsZG5CbHMtZHpYX3Y3elI3TkQ1d0lCQzgxeVhrZU9yRno4VUlXcXVJUUI4YUxuTjJhN2RiQnIwNUJhRXVmaDhXSTNPZ0xrRjVHeEc0ME91cmZBRlNnRy1hdnVRNmVzVE9uR0JQUkNsaElMbVY5WFB3NEJPQT1BN0M4MUMzMw==";
                options.JiraDomain = "https://kba5859.atlassian.net";
            });

            // Регистрация Salesforce и Jira сервисов
            builder.Services.AddScoped<ISalesforceService, SalesforceService>();
            builder.Services.AddScoped<IJiraService, JiraService>();

            await builder.Build().RunAsync();
        }
    }
}
