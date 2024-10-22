using Blazored.LocalStorage;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Forms.Models;

namespace FormsClone.CSharp.MainFunctionality.Forms.Services
{
    public class FormsService : BaseEntityService<Form>
    {
        protected override string Prefix => "form_";

        public FormsService(ILocalStorageService localStorage) : base(localStorage)
        {
        }
    }
}
