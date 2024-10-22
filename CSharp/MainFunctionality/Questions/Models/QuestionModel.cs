using System.ComponentModel.DataAnnotations;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Interfaces;

namespace FormsClone.CSharp.MainFunctionality.Questions.Models
{
    public class Question : BaseEntityModel, IEntityModel
    {
        [Required(ErrorMessage = "Текст вопроса является обязательным.")]
        public string? Text { get; set; } // Текст вопроса

        public string? Info { get; set; } // Дополнительная информация о вопросе

        [Required(ErrorMessage = "Тип вопроса является обязательным.")]
        public string Type { get; set; } = string.Empty; // Тип вопроса (например, текстовый, выбор, и т. д.)
    }
}
