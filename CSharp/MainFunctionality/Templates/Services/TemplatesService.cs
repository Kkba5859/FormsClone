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

        public override async Task<List<Template>> SearchAsync(string searchTerm)
        {
            var allTemplates = await GetAllAsync(); 
            return allTemplates.Where(t =>
                t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (t.Description != null && t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) // Поиск по заголовку и описанию
                ).ToList();
        }
    }
}
