using System.ComponentModel.DataAnnotations;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Interfaces;

namespace FormsClone.CSharp.MainFunctionality.Forms.Models
{
    public class Form : BaseEntityModel, IEntityModel
    {
        [Required(ErrorMessage = "Название формы обязательно для заполнения.")]
        public string? Name { get; set; } 

        public string? Info { get; set; } 
    }
}
