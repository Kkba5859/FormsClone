using Blazored.LocalStorage;

namespace FormsClone.Models.Templates.Model
{
    public class TemplatesModel : ITemplatesModel
    {
        private const string TemplatesKey = "templates";
        private readonly ILocalStorageService _localStorage;

        public TemplatesModel(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<Template>> GetTemplatesAsync()
        {
            var templates = await _localStorage.GetItemAsync<List<Template>>(TemplatesKey);
            return templates ?? new List<Template>();
        }

        public async Task<Template?> GetTemplateByIdAsync(int id)
        {
            var templates = await GetTemplatesAsync();
            return templates.FirstOrDefault(t => t.Id == id);
        }

        public async Task SaveTemplateAsync(Template template)
        {
            var templates = await GetTemplatesAsync();

            if (template.Id == 0)  // Новый шаблон
            {
                template.Id = templates.Count > 0 ? templates.Max(t => t.Id) + 1 : 1;
                templates.Add(template);
            }
            else  // Обновление существующего шаблона
            {
                var existingTemplate = templates.FirstOrDefault(t => t.Id == template.Id);
                if (existingTemplate != null)
                {
                    templates.Remove(existingTemplate); // Удаляем старую версию
                    templates.Add(template); // Добавляем обновленную
                }
            }

            await _localStorage.SetItemAsync(TemplatesKey, templates);
        }

        public async Task DeleteTemplateAsync(int id)
        {
            var templates = await GetTemplatesAsync();
            var templateToDelete = templates.FirstOrDefault(t => t.Id == id);

            if (templateToDelete != null)
            {
                templates.Remove(templateToDelete);
                await _localStorage.SetItemAsync(TemplatesKey, templates);
            }
        }
    }
}
