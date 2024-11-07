using FormsClone.CSharp.UserManagement.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly IJiraService _jiraService;

        public UserDashboardService(HttpClient httpClient, IJiraService jiraService)
        {
            _httpClient = httpClient;
            _jiraService = jiraService;
        }

        public async Task<string> CreateJiraIssueAsync(string summary, string description, string priority, string status, string template)
        {
            // Вызов сервиса для создания задачи с дополнительными параметрами
            var ticketLink = await _jiraService.CreateJiraIssueAsync(summary, description, priority, status, template);

            return ticketLink;
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            var content = new StringContent($"{{\"username\": \"{username}\", \"newPassword\": \"{newPassword}\"}}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/ChangePassword", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(string username)
        {
            var response = await _httpClient.DeleteAsync($"api/User/Delete/{username}");
            response.EnsureSuccessStatusCode();
        }
    }
}
