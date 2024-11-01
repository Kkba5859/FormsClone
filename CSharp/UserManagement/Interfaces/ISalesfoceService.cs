namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface ISalesforceService
    {
        Task<string> GetSalesforceDataAsync(string accessToken, string query);
        Task<string> GetAccessToken(string username, string password);
    }
}
