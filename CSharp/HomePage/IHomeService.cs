using FormsClone.CSharp.MainFunctionality.Forms.Models; // Подключаем модели форм, используемые в интерфейсе.
using FormsClone.CSharp.MainFunctionality.Templates.Models; // Подключаем модели шаблонов, необходимые для методов интерфейса.
using FormsClone.CSharp.MainFunctionality.Questions.Models; // Подключаем модели вопросов, применяемые в методах интерфейса.

namespace FormsClone.CSharp.HomePage // Определяем пространство имен для главной страницы, в котором будет находиться интерфейс IHomeService.
{
    public interface IHomeService // Объявляем интерфейс IHomeService, описывающий функциональность, необходимую для работы главной страницы.
    {
        Task<List<Form>> GetAllFormsAsync(); // Асинхронный метод для получения всех форм, возвращающий список объектов Form.

        Task<List<Template>> GetAllTemplatesAsync(); // Асинхронный метод для получения всех шаблонов, возвращающий список объектов Template.

        Task<List<Question>> GetAllQuestionsAsync(); // Асинхронный метод для получения всех вопросов, возвращающий список объектов Question.

        Task DeleteFormAsync(string id); // Асинхронный метод для удаления формы по идентификатору id, переданному в виде строки.

        Task DeleteTemplateAsync(string id); // Асинхронный метод для удаления шаблона по идентификатору id, переданному в виде строки.

        Task DeleteQuestionAsync(string id); // Асинхронный метод для удаления вопроса по идентификатору id, переданному в виде строки.
    }
}
