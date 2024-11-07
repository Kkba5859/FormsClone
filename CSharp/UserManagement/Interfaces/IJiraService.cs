using FormsClone.CSharp.UserManagement.AdminDashboard.Models;
using FormsClone.CSharp.UserManagement.AdminDashboard.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IJiraService
    {
        // Получение задач из Jira
        Task<List<IssueModel>> GetJiraIssuesAsync();

        // Создание новой задачи в Jira с дополнительными параметрами
        Task<string> CreateJiraIssueAsync(string summary, string description, string priority, string status, string template);
    }
}
