namespace FormsClone.CSharp.MainFunctionality.Interfaces // Пространство имен для интерфейсов бизнес-логики
{
    public interface ITabService // Интерфейс для сервиса вкладок
    {
        string ActiveTab { get; set; } // Свойство для получения или установки активной вкладки
        void SetActiveTab(string tab); // Метод для установки активной вкладки
    }
}
