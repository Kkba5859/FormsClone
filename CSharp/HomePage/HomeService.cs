using FormsClone.CSharp.MainFunctionality.Interfaces;
using FormsClone.CSharp.MainFunctionality.Forms.Models;
using FormsClone.CSharp.MainFunctionality.Templates.Models;
using FormsClone.CSharp.MainFunctionality.Questions.Models;

namespace FormsClone.CSharp.HomePage
{
    public class HomeService : IHomeService
    {
        private readonly IEntityService<Form> _formsService;
        private readonly IEntityService<Template> _templatesService;
        private readonly IEntityService<Question> _questionsService;

        public HomeService(IEntityService<Form> formsService, IEntityService<Template> templatesService, IEntityService<Question> questionsService)
        {
            _formsService = formsService;
            _templatesService = templatesService;
            _questionsService = questionsService;
        }

        public async Task<List<Form>> GetAllFormsAsync()
        {
            return await _formsService.GetAllAsync();
        }

        public async Task<List<Template>> GetAllTemplatesAsync()
        {
            return await _templatesService.GetAllAsync();
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionsService.GetAllAsync();
        }

        public async Task DeleteFormAsync(string id)
        {
            await _formsService.DeleteAsync(int.Parse(id));
        }

        public async Task DeleteTemplateAsync(string id)
        {
            await _templatesService.DeleteAsync(int.Parse(id));
        }

        public async Task DeleteQuestionAsync(string id)
        {
            await _questionsService.DeleteAsync(int.Parse(id));
        }
    }
}
