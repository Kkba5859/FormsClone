using Blazored.LocalStorage; // Подключение библиотеки для работы с локальным хранилищем.
using FormsClone.CSharp.MainFunctionality.AbstractClasses; // Подключение абстрактного класса для базового сервиса.
using FormsClone.CSharp.MainFunctionality.Forms.Models; // Подключение модели Form.


namespace FormsClone.CSharp.MainFunctionality.Forms.Services
{
    public class FormsService : BaseEntityService<Form> // Класс для работы с сущностями типа Form.
    {
        protected override string Prefix => "form_"; // Префикс для ключей в локальном хранилище, идентифицирующий формы.

        // Конструктор класса, принимающий ILocalStorageService и передающий его базовому классу.
        public FormsService(ILocalStorageService localStorage) : base(localStorage)
        {
        }

        // Переопределение метода для поиска форм по ключевому слову.
        public override async Task<List<Form>> SearchAsync(string searchTerm)
        {
            var allForms = await GetAllAsync(); // Получаем все формы из локального хранилища.

            // Фильтруем формы по совпадению в полях Name и Info с ключевым словом.
            return allForms.Where(f =>
                f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || // Проверка на совпадение в поле Name.
                (f.Info != null && f.Info.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))) // Проверка на совпадение в поле Info.
                .ToList(); // Возвращаем отфильтрованный список.
        }
    }
}
