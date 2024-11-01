using FormsClone.Pages.MainFunctionality.Forms.CreateForm.Tabs.Questions.SelectComponents.FormsClone.CSharp.MainFunctionality.Interfaces; // Подключение пространства имен для интерфейсов

namespace FormsClone.CSharp.MainFunctionality.Forms.Services // Пространство имен для компонентов выбора вопросов
{
    public class QuestionService : IQuestionService // Реализация интерфейса IQuestionService
    {
        public async Task<List<string>> GetDefaultOptionsAsync(string questionType) // Асинхронный метод для получения стандартных вариантов в зависимости от типа вопроса
        {
            return questionType switch // Использование выражения switch для выбора вариантов
            {
                "Один из списка" => new List<string> { "Вариант 1" }, // Для "Один из списка" возвращаем стандартный вариант
                "Несколько из списка" => new List<string> { "Вариант 1" }, // Для "Несколько из списка" возвращаем стандартный вариант
                _ => new List<string>() // Для других типов возвращаем пустой список
            };
        }

        public async Task<string> GetDefaultFieldLabelAsync(string questionType) // Асинхронный метод для получения стандартной метки поля в зависимости от типа вопроса
        {
            return questionType switch // Использование выражения switch для выбора метки поля
            {
                "Текст" => "Текст", // Для типа "Текст" возвращаем метку "Текст"
                "Один из списка" => "Один из списка", // Для типа "Один из списка" возвращаем соответствующую метку
                "Несколько из списка" => "Несколько из списка", // Для типа "Несколько из списка" возвращаем соответствующую метку
                "Шкала" => "Шкала", // Для типа "Шкала" возвращаем соответствующую метку
                _ => "Вопрос" // Для других типов возвращаем метку "Вопрос"
            };
        }

        public async Task<int> GetMaxOptionsCountAsync() => 50; // Асинхронный метод для получения максимального количества вариантов (50)
    }
}
