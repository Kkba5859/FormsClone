namespace FormsClone.Pages.MainFunctionality.Forms.CreateForm.Tabs.Questions.SelectComponents // Пространство имен для компонентов выбора вопросов
{
    namespace FormsClone.CSharp.MainFunctionality.Interfaces // Вложенное пространство имен для интерфейсов бизнес-логики
    {
        public interface IQuestionService // Интерфейс для сервиса вопросов
        {
            Task<List<string>> GetDefaultOptionsAsync(string questionType); // Метод для получения списка вариантов по типу вопроса
            Task<string> GetDefaultFieldLabelAsync(string questionType); // Метод для получения метки поля по типу вопроса
            Task<int> GetMaxOptionsCountAsync(); // Метод для получения максимального количества вариантов
        }
    }
}
