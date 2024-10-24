using FormsClone.CSharp.MainFunctionality.Forms.Models;
using FormsClone.CSharp.MainFunctionality.Templates.Models;
using FormsClone.CSharp.MainFunctionality.Questions.Models;

namespace FormsClone.CSharp.HomePage
{
    public interface IHomeService
    {
        Task<List<Form>> GetAllFormsAsync();
        Task<List<Template>> GetAllTemplatesAsync();
        Task<List<Question>> GetAllQuestionsAsync();
        Task DeleteFormAsync(string id);
        Task DeleteTemplateAsync(string id);
        Task DeleteQuestionAsync(string id);
    }
}
