using Blazored.LocalStorage; // Подключение пространства имен для работы с локальным хранилищем.
using FormsClone.CSharp.MainFunctionality.AbstractClasses; // Подключение абстрактного класса для базовой модели сущности.
using FormsClone.CSharp.MainFunctionality.Templates.Models; // Подключение модели шаблона.
using FormsClone.CSharp.MainFunctionality.Interfaces; // Подключение интерфейсов.

namespace FormsClone.CSharp.MainFunctionality.Templates.Services
{
    public class TemplatesService : BaseEntityService<Template> // Класс TemplatesService наследуется от базовой сущности для работы с шаблонами.
    {
        protected override string Prefix => "template_"; // Определение префикса для ключей в локальном хранилище.

        public TemplatesService(ILocalStorageService localStorage) : base(localStorage) // Конструктор, принимающий сервис локального хранилища.
        {
        }

        public override async Task<List<Template>> SearchAsync(string searchTerm) // Переопределение метода поиска шаблонов по строке запроса.
        {
            var allTemplates = await GetAllAsync(); // Получение всех шаблонов из локального хранилища.
            return allTemplates.Where(t => // Фильтрация шаблонов по содержимому названия или описания.
                t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || // Проверка, содержит ли название шаблона строку запроса без учета регистра.
                (t.Description != null && t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || // Проверка, содержит ли описание шаблона строку запроса без учета регистра.
                t.Theme != null && t.Theme.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || // Проверка, содержит ли тематика шаблона строку запроса без учета регистра.
                t.Tags != null && t.Tags.Any(tag => tag.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))) // Проверка, содержит ли хотя бы один тег строку запроса без учета регистра.
                )
                .ToList(); // Преобразование отфильтрованных шаблонов в список.
        }
    }
}
