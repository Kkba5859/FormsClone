using FormsClone.CSharp.MainFunctionality.Interfaces; // Подключаем интерфейсы основной функциональности, которые используются сервисами для управления сущностями.
using FormsClone.CSharp.MainFunctionality.Forms.Models; // Подключаем модели форм, которые используются в методах этого сервиса.
using FormsClone.CSharp.MainFunctionality.Templates.Models; // Подключаем модели шаблонов, которые будут обрабатываться сервисом.
using FormsClone.CSharp.MainFunctionality.Questions.Models; // Подключаем модели вопросов для использования в методах сервиса.

namespace FormsClone.CSharp.HomePage // Объявляем пространство имен для сервиса главной страницы.
{
    public class HomeService : IHomeService // Определяем класс HomeService, который реализует интерфейс IHomeService.
    {
        // Поля только для чтения, представляющие сервисы для работы с формами, шаблонами и вопросами.
        private readonly IEntityService<Form> _formsService; // Поле для работы с сервисом форм.
        private readonly IEntityService<Template> _templatesService; // Поле для работы с сервисом шаблонов.
        private readonly IEntityService<Question> _questionsService; // Поле для работы с сервисом вопросов.

        // Конструктор класса HomeService, который принимает зависимости для работы с формами, шаблонами и вопросами.
        public HomeService(IEntityService<Form> formsService, IEntityService<Template> templatesService, IEntityService<Question> questionsService)
        {
            _formsService = formsService; // Инициализируем поле _formsService, передавая сервис для работы с формами.
            _templatesService = templatesService; // Инициализируем поле _templatesService, передавая сервис для работы с шаблонами.
            _questionsService = questionsService; // Инициализируем поле _questionsService, передавая сервис для работы с вопросами.
        }

        // Асинхронный метод для получения всех форм.
        public async Task<List<Form>> GetAllFormsAsync()
        {
            return await _formsService.GetAllAsync(); // Вызываем метод GetAllAsync() из _formsService для получения всех форм.
        }

        // Асинхронный метод для получения всех шаблонов.
        public async Task<List<Template>> GetAllTemplatesAsync()
        {
            return await _templatesService.GetAllAsync(); // Вызываем метод GetAllAsync() из _templatesService для получения всех шаблонов.
        }

        // Асинхронный метод для получения всех вопросов.
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionsService.GetAllAsync(); // Вызываем метод GetAllAsync() из _questionsService для получения всех вопросов.
        }

        // Асинхронный метод для удаления формы по ее идентификатору.
        public async Task DeleteFormAsync(string id)
        {
            await _formsService.DeleteAsync(int.Parse(id)); // Преобразуем строку id в int и передаем ее в метод DeleteAsync() для удаления формы.
        }

        // Асинхронный метод для удаления шаблона по его идентификатору.
        public async Task DeleteTemplateAsync(string id)
        {
            await _templatesService.DeleteAsync(int.Parse(id)); // Преобразуем строку id в int и передаем ее в метод DeleteAsync() для удаления шаблона.
        }

        // Асинхронный метод для удаления вопроса по его идентификатору.
        public async Task DeleteQuestionAsync(string id)
        {
            await _questionsService.DeleteAsync(int.Parse(id)); // Преобразуем строку id в int и передаем ее в метод DeleteAsync() для удаления вопроса.
        }
    }
}
