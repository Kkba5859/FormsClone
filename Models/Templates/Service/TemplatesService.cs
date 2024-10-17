using Blazored.LocalStorage;
using FormsClone.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TemplatesService : ITemplatesService
{
    private const string TemplatePrefix = "template_";
    private readonly ILocalStorageService _localStorage;

    public TemplatesService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task CreateTemplateAsync(Template template)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));

        var templates = await GetTemplatesAsync();
        template.Id = templates.Count > 0 ? templates.Max(t => t.Id) + 1 : 1; // Уникальный ID для шаблона
        templates.Add(template);
        await SaveTemplatesAsync(templates); // Сохраняем все шаблоны
    }

    public async Task<List<Template>> GetTemplatesAsync()
    {
        var templates = new List<Template>();
        var keys = await _localStorage.KeysAsync();
        foreach (var key in keys)
        {
            if (key.StartsWith(TemplatePrefix)) // Получаем только ключи шаблонов
            {
                var template = await _localStorage.GetItemAsync<Template>(key);
                if (template != null)
                {
                    templates.Add(template);
                }
            }
        }
        return templates;
    }

    public async Task<Template> GetTemplateByIdAsync(int templateId)
    {
        var templates = await GetTemplatesAsync();
        return templates.FirstOrDefault(t => t.Id == templateId) ?? throw new InvalidOperationException("Шаблон не найден.");
    }

    public async Task UpdateTemplateAsync(Template template)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));

        var existingTemplate = await GetTemplateByIdAsync(template.Id);
        existingTemplate.Title = template.Title;
        existingTemplate.Description = template.Description;
        existingTemplate.Theme = template.Theme;
        existingTemplate.Tags = template.Tags;

        await SaveTemplateAsync(existingTemplate); // Сохраняем обновленный шаблон
    }

    public async Task DeleteTemplateAsync(int templateId)
    {
        var templateKey = TemplatePrefix + templateId; // Формируем ключ для удаления
        var existingTemplate = await _localStorage.GetItemAsync<Template>(templateKey);
        if (existingTemplate != null)
        {
            await _localStorage.RemoveItemAsync(templateKey); // Удаляем шаблон по ключу
        }
    }

    private async Task SaveTemplatesAsync(List<Template> templates)
    {
        foreach (var template in templates)
        {
            var templateKey = TemplatePrefix + template.Id; // Формируем ключ для сохранения
            await _localStorage.SetItemAsync(templateKey, template); // Сохраняем каждый шаблон по отдельности
        }
    }

    private async Task SaveTemplateAsync(Template template)
    {
        var templateKey = TemplatePrefix + template.Id; // Формируем ключ для сохранения
        await _localStorage.SetItemAsync(templateKey, template); // Сохраняем шаблон
    }
}
