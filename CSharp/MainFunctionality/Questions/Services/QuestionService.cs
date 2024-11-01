using Blazored.LocalStorage; // Подключение пространства имен для работы с локальным хранилищем.
using FormsClone.CSharp.MainFunctionality.AbstractClasses; // Подключение абстрактного класса для базовой модели сущности.
using FormsClone.CSharp.MainFunctionality.Questions.Models; // Подключение модели вопроса.

namespace FormsClone.CSharp.MainFunctionality.Questions.Services
{
    public class QuestionsService : BaseEntityService<Question> // Класс QuestionsService наследуется от базовой сущности для работы с вопросами.
    {
        protected override string Prefix => "question_"; // Определение префикса для ключей в локальном хранилище.

        public QuestionsService(ILocalStorageService localStorage) : base(localStorage) // Конструктор, принимающий сервис локального хранилища.
        {
        }

        public override async Task<List<Question>> SearchAsync(string searchTerm) // Переопределение метода поиска вопросов по строке запроса.
        {
            var allQuestions = await GetAllAsync(); // Получение всех вопросов из локального хранилища.
            return allQuestions.Where(q => // Фильтрация вопросов по содержимому текста.
                q.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) // Проверка, содержит ли текст вопроса строку запроса без учета регистра.
                ).ToList(); // Преобразование отфильтрованных вопросов в список.
        }
    }
}
