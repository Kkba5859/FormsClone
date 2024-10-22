using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FormsClone.CSharp.MainFunctionality.Templates.Services;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using FormsClone.CSharp.MainFunctionality.Questions.Services;
using FormsClone.CSharp.MainFunctionality.Forms.Services;
using FormsClone.CSharp.UserManagement.Interfaces;
using FormsClone.CSharp.MainFunctionality.Interfaces;
using FormsClone.CSharp.MainFunctionality.Forms.Models;
using FormsClone.CSharp.MainFunctionality.Questions.Models;
using FormsClone.CSharp.MainFunctionality.Templates.Models;
using FormsClone.Temporary;

namespace FormsClone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // ���������� LocalStorage ��� ���������� �������� �������������
            builder.Services.AddBlazoredLocalStorage();

            // ����������� ��������� �����������
            builder.Services.AddSingleton<AuthStateService>();

            // ����������� ������� ��� ������ � �������������� (� ����������������)
            builder.Services.AddScoped<IUserService, UserService>();

            // ����������� ������� ��� ������ � ������������
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            // ����������� �������� ��� ������ � �������, ��������� � ��������� ����� IEntityService
            builder.Services.AddScoped<IEntityService<Form>, FormsService>();
            builder.Services.AddScoped<IEntityService<Question>, QuestionsService>();
            builder.Services.AddScoped<IEntityService<Template>, TemplatesService>();

            await builder.Build().RunAsync();
        }
    }
}
