using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
using System.Net.Http.Headers;
using System.Text;

namespace FormsClone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient();

            // Register local storage
            builder.Services.AddBlazoredLocalStorage();

            // Register authentication and user management services
            builder.Services.AddSingleton<AuthStateService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IUserDashboardService, UserDashboardService>();

            // Register entity services for "Form", "Question", and "Template"
            builder.Services.AddScoped<IEntityService<Form>, FormsService>();
            builder.Services.AddScoped<IEntityService<Question>, QuestionsService>();
            builder.Services.AddScoped<IEntityService<Template>, TemplatesService>();

            // Register other required services
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ITabService, TabService>();

            builder.Services.AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri("https://kba5859.atlassian.net/") });

            // Authentication credentials for Jira
            string username = "kkba5859";
            string apiToken = "ATATT3xFfGF0zcamIJKlaeNNXPCzq-cHQc8nmR7xFH1UdCoFyzElfPB9x04P77ZZiY7ldnBls-dzX_v7zR7ND5wIBC81yXkeOrFz8UIWquIQB8aLnN2a7dbBr05BaEufh8WI3OgLkF5GxG40OurfAFSgG-avuQ6esTOnGBPRClhILmV9XPw4BOA=A7C81C33";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{apiToken}"));

            // Base URL for Jira API
            string jiraDomain = "https://kba5859.atlassian.net/";

            // Register JiraService with encoded credentials and base URL for the JiraProxyAPI
            string jiraProxyApiUrl = "https://jiraproxyapi20241105135746.azurewebsites.net"; // URL вашего API

            // Register JiraService in DI with necessary parameters
            builder.Services.AddScoped<IJiraService, JiraService>(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();  // Get the already configured HttpClient
                return new JiraService(httpClient, jiraProxyApiUrl);  // Pass only HttpClient and Proxy URL
            });


            await builder.Build().RunAsync();
        }
    }
}
