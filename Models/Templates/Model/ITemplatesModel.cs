namespace FormsClone.Models.Templates.Model
{
    public interface ITemplatesModel
    {
        Task<List<Template>> GetTemplatesAsync(); // Получение всех шаблонов
        Task<Template?> GetTemplateByIdAsync(int id); // Получение шаблона по ID
        Task SaveTemplateAsync(Template template); // Сохранение шаблона (создание или обновление)
        Task DeleteTemplateAsync(int id); // Удаление шаблона по ID
    }
}