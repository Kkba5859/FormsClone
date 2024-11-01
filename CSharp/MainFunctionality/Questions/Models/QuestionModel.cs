using System.ComponentModel.DataAnnotations; // Подключение пространства имен для использования атрибутов валидации.
using FormsClone.CSharp.MainFunctionality.AbstractClasses; // Подключение абстрактного класса для базовой модели сущности.
using FormsClone.CSharp.MainFunctionality.Interfaces; // Подключение интерфейса для модели сущности.

namespace FormsClone.CSharp.MainFunctionality.Questions.Models
{
    public class Question : BaseEntityModel, IEntityModel // Класс Question наследуется от базовой модели и реализует интерфейс модели сущности.
    {
        [Required(ErrorMessage = "Текст вопроса является обязательным.")] // Атрибут, указывающий, что свойство обязательно для заполнения.
        public string? Text { get; set; } // Свойство для текста вопроса, может быть пустым или null.

        public string? Info { get; set; } // Дополнительная информация по вопросу, может быть пустым или null.

        [Required(ErrorMessage = "Тип вопроса является обязательным.")] // Атрибут, указывающий, что тип вопроса обязателен для заполнения.
        public string Type { get; set; } = string.Empty; // Свойство для типа вопроса, по умолчанию пустая строка.
    }
}
