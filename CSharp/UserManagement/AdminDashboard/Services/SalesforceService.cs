using FormsClone.CSharp.UserManagement.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class SalesforceService : ISalesforceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SalesforceService> _logger;

        private const string ConsumerKey = "3MVG9WktJx3rjNu3I_jgw2Qz4x6_GBZHjTST5.f9oNKHMuGYvXi2RzrKAq29O.Hf9Gwr0RdX8NnXACirOMius";
        private const string ConsumerSecret = "847B315F8B9364D7A085E197B2B312F3FE5C47B6DDEBB352822CA80B235087CE";

        public SalesforceService(HttpClient httpClient, ILogger<SalesforceService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetSalesforceDataAsync(string accessToken, string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"flow-ability-1520.my.salesforce.com/services/data/vXX.X/query/?q={query}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            _logger.LogInformation("Sending request to Salesforce with query: {Query}", query);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<dynamic>();
                _logger.LogInformation("Successfully fetched Salesforce data.");
                return data?.ToString() ?? string.Empty; // Верните данные в нужном формате
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error fetching Salesforce data: {StatusCode} - {ErrorResponse}", response.StatusCode, errorResponse);
                throw new HttpRequestException($"Error fetching Salesforce data: {response.StatusCode}");
            }
        }

        public async Task<string> GetAccessToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            var securityToken = "3Hhee460l6uA7SQ7ASqbhuYCj";
            var combinedPassword = password + securityToken;

            var body = new Dictionary<string, string>
    {
        { "grant_type", "password" },
        { "client_id", ConsumerKey },
        { "client_secret", ConsumerSecret },
        { "username", username },
        { "password", combinedPassword }
    };

            var request = new HttpRequestMessage(HttpMethod.Post, $"flow-ability-1520.my.salesforce.com/services/oauth2/token")
            {
                Content = new FormUrlEncodedContent(body)
            };

            _logger.LogInformation("Requesting access token for user: {Username}", username);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (tokenResponse != null && tokenResponse.TryGetValue("access_token", out var accessToken))
                {
                    _logger.LogInformation("Successfully fetched access token.");
                    return accessToken;
                }
                else
                {
                    _logger.LogError("Access token not found in the response.");
                    throw new Exception("Access token not found in the response.");
                }
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error fetching access token: {StatusCode} - {ErrorResponse}", response.StatusCode, errorResponse);
                throw new HttpRequestException($"Error fetching access token: {response.StatusCode}");
            }
        }

    }
}
