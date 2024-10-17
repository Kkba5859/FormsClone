using FormsClone.Models.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITemplatesService
{
    Task CreateTemplateAsync(Template template);
    Task<List<Template>> GetTemplatesAsync();
    Task<Template> GetTemplateByIdAsync(int templateId);
    Task UpdateTemplateAsync(Template template);
    Task DeleteTemplateAsync(int templateId);
}