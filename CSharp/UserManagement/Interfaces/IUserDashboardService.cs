using System.Threading.Tasks;

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IUserDashboardService
    {
        // Обновленный метод для создания задачи в Jira с дополнительными параметрами
        Task<string> CreateJiraIssueAsync(string summary, string description, string priority, string status, string template);

        Task ChangePasswordAsync(string username, string newPassword);
        Task DeleteUserAsync(string username);
    }
}
