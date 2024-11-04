using System.Net.Http.Headers;
using System.Text;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services.FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using FormsClone.CSharp.UserManagement.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class JiraService : IJiraService
    {
        private readonly HttpClient _httpClient;
        private readonly JiraSettings _settings;

        public JiraService(HttpClient httpClient, IOptions<JiraSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _settings.EncodedCredentials);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CreateJiraIssueAsync(string projectKey, string summary, string description, string issueType)
        {
            var url = $"{_settings.JiraDomain}/rest/api/3/issue";

            var issueData = new JObject
            {
                ["fields"] = new JObject
                {
                    ["project"] = new JObject { ["key"] = projectKey },
                    ["summary"] = summary,
                    ["description"] = description,
                    ["issuetype"] = new JObject { ["name"] = issueType }
                }
            };

            var content = new StringContent(issueData.ToString(), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Задача успешно создана.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка создания задачи: {response.StatusCode}. Ответ: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при создании задачи: {ex.Message}");
            }
        }

    }
}
