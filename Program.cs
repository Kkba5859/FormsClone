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

            // ���������� SQLite ��� ���������� �������� ������
            var connectionString = "Data Source=FormsClone.db"; // ���������, ��� ���� ���������
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddSingleton<AuthStateService>();

            // ���������� LocalStorage ��� ���������� �������� �������������
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddBlazoredModal();

            builder.Services.AddScoped<ITemplatesService, TemplatesService>();

            // ����������� ������������� ������� ��� ������ � �������������� (� ����������������)
            builder.Services.AddScoped<IUserService, UserService>();


            // ����������� ������ ��� ������ � ������������
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            await builder.Build().RunAsync();
        }
    }
}