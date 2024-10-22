using Blazored.LocalStorage;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Templates.Models;

namespace FormsClone.CSharp.MainFunctionality.Templates.Services
{
    public class TemplatesService : BaseEntityService<Template>
    {
        protected override string Prefix => "template_";

        public TemplatesService(ILocalStorageService localStorage) : base(localStorage)
        {
        }
    }
}
