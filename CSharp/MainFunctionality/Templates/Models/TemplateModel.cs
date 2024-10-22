using System.ComponentModel.DataAnnotations;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Interfaces;

namespace FormsClone.CSharp.MainFunctionality.Templates.Models
{
    public class Template : BaseEntityModel, IEntityModel
    {
        [Required(ErrorMessage = "Название шаблона обязательно для заполнения.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Описание шаблона обязательно для заполнения.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Тематика шаблона обязательна для заполнения.")]
        public string? Theme { get; set; }

        [Required(ErrorMessage = "Теги обязательны для заполнения.")]
        public string[]? Tags { get; set; }
    }
}
