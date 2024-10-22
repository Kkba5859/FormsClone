using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FormsClone.CSharp.MainFunctionality.Interfaces;
using FormsClone.CSharp.MainFunctionality.Forms.Models;
using FormsClone.CSharp.MainFunctionality.Forms.Services;
using FormsClone.CSharp.MainFunctionality.Questions.Models;
using FormsClone.CSharp.MainFunctionality.Questions.Services;
using FormsClone.CSharp.MainFunctionality.Templates.Models;
using FormsClone.CSharp.MainFunctionality.Templates.Services;
using FormsClone.Models.Registration.Service;
using FormsClone.Models.User.Service;


namespace FormsClone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // ƒобавление LocalStorage дл€ локального хранени€ пользователей
            builder.Services.AddBlazoredLocalStorage();

            // –егистраци€ состо€ни€ авторизации
            builder.Services.AddSingleton<AuthStateService>();

            // –егистраци€ сервиса дл€ работы с пользовател€ми (и администраторами)
            builder.Services.AddScoped<IUserService, UserService>();

            // –егистраци€ сервиса дл€ работы с регистрацией
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            // –егистраци€ сервисов дл€ работы с формами, вопросами и шаблонами через IEntityService
            builder.Services.AddScoped<IEntityService<Form>, FormsService>();
            builder.Services.AddScoped<IEntityService<Question>, QuestionsService>();
            builder.Services.AddScoped<IEntityService<Template>, TemplatesService>();

            await builder.Build().RunAsync();
        }
    }
}
