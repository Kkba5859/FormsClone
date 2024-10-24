using System.ComponentModel.DataAnnotations;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Interfaces;

namespace FormsClone.CSharp.MainFunctionality.Questions.Models
{
    public class Question : BaseEntityModel, IEntityModel
    {
        [Required(ErrorMessage = "Текст вопроса является обязательным.")]
        public string? Text { get; set; } 

        public string? Info { get; set; } 

        [Required(ErrorMessage = "Тип вопроса является обязательным.")]
        public string Type { get; set; } = string.Empty; 
    }
}
