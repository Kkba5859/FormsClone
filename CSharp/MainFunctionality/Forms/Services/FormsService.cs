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

    
        public override async Task<List<Form>> SearchAsync(string searchTerm)
        {
            var allForms = await GetAllAsync(); 
            return allForms.Where(f =>
                f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (f.Info != null && f.Info.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))) 
                .ToList(); 
        }
    }
}
