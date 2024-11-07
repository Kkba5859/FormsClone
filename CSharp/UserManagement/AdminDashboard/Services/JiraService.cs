using System.Net.Http.Headers;
using System.Text;
using FormsClone.CSharp.UserManagement.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using FormsClone.CSharp.UserManagement.AdminDashboard.Models;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Services
{
    public class JiraService : IJiraService
    {
        private readonly HttpClient _httpClient;
        private readonly string _jiraProxyApiUrl;

        public JiraService(HttpClient httpClient, string jiraProxyApiUrl)
        {
            _httpClient = httpClient;
            _jiraProxyApiUrl = jiraProxyApiUrl;
        }

        // Получение задач из Jira
        public async Task<List<IssueModel>> GetJiraIssuesAsync()
        {
            var issues = new List<IssueModel>();

            try
            {
                // Делаем запрос к серверу, который проксирует запросы в Jira
                var response = await _httpClient.GetAsync($"{_jiraProxyApiUrl}/api/fetchissues/issues");

                if (response.IsSuccessStatusCode)
                {
                    // Чтение ответа от Jira
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Ответ от Jira: " + responseBody); // Логируем ответ

                    // Парсим ответ
                    var jiraData = JsonConvert.DeserializeObject<JiraSearchResponse>(responseBody);

                    if (jiraData?.Issues != null && jiraData.Issues.Count > 0)
                    {
                        // Добавляем задачи в список
                        foreach (var issue in jiraData.Issues)
                        {
                            issues.Add(new IssueModel
                            {
                                Key = issue.Key ?? "Не указан",  // Если нет ключа задачи, ставим "Не указан"
                                Summary = issue.Fields?.Summary ?? "Не указано",  // Если нет summary, ставим "Не указано"
                                Description = issue.Fields?.Description ?? "Не указано",  // Если нет описания, ставим "Не указано"
                                TicketLink = issue.Key != null ? $"https://kba5859.atlassian.net/browse/{issue.Key}" : "Ссылка недоступна",  // Ссылка на задачу в Jira
                                Status = issue.Fields?.Status?.Name ?? "Не указан",  // Если статус не найден, ставим "Не указан"
                                Assignee = issue.Fields?.Assignee?.DisplayName ?? "Не назначен",  // Если исполнитель не найден, ставим "Не назначен"
                                Reporter = issue.Fields?.Reporter?.DisplayName ?? "Не указан",  // Если репортер не найден, ставим "Не указан"
                                Created = issue.Fields?.Created ?? "Не указана" // Если дата создания не найдена, ставим "Не указана"
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет задач в ответе от Jira.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка при загрузке задач: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении задач из Jira: {ex.Message}");
            }

            return issues;
        }

        // Создание новой задачи в Jira
        public async Task<string> CreateJiraIssueAsync(string summary, string description, string priority, string status, string template)
        {
            var issueData = new
            {
                Summary = summary,
                Description = description,
                Priority = priority,  // Новый параметр приоритет
                Status = status,      // Новый параметр статус
                Template = template   // Новый параметр шаблон
            };

            var jsonContent = JsonConvert.SerializeObject(issueData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_jiraProxyApiUrl}/api/Jira/CreateTicket", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    return jsonResponse?.TicketLink ?? string.Empty;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка от Jira API: {errorResponse}");
                    throw new Exception($"Ошибка при создании тикета. Ответ сервера: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании тикета: {ex.Message}");
                throw new Exception("Произошла ошибка при попытке создать тикет.", ex);
            }
        }
    }

    // Ответ от Jira с задачами
    public class JiraSearchResponse
    {
        public List<JiraIssue>? Issues { get; set; }
    }

    // Задача Jira
    public class JiraIssue
    {
        public string? Key { get; set; }
        public JiraIssueFields Fields { get; set; }
    }

    // Поля задачи Jira
    public class JiraIssueFields
    {
        public string? Summary { get; set; }
        public JiraIssueStatus? Status { get; set; }
        public JiraAssignee? Assignee { get; set; }
        public JiraAssignee? Reporter { get; set; }
        public string? Description { get; set; }  // Описание задачи
        public string? Created { get; set; }      // Дата создания задачи
    }

    // Статус задачи
    public class JiraIssueStatus
    {
        public string? Name { get; set; }
    }

    // Назначенный исполнитель или создатель задачи
    public class JiraAssignee
    {
        public string? DisplayName { get; set; }
    }

    // Модель задачи для отображения в приложении
    public class IssueModel
    {
        public string? Key { get; set; }          // Уникальный идентификатор задачи
        public string? Summary { get; set; }      // Краткое описание задачи
        public string? Description { get; set; }  // Полное описание задачи
        public string? Assignee { get; set; }     // Исполнитель задачи
        public string? Reporter { get; set; }     // Создатель задачи
        public string? Created { get; set; }      // Дата создания задачи
        public string? TicketLink { get; set; }   // Ссылка на задачу в Jira
        public string? Status { get; set; }       // Статус задачи
    }
}
