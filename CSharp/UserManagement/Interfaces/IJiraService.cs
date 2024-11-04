namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IJiraService
    {
        Task CreateJiraIssueAsync(string projectKey, string summary, string description, string issueType);
    }
}
