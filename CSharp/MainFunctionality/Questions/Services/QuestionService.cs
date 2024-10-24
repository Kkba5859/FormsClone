using Blazored.LocalStorage;
using FormsClone.CSharp.MainFunctionality.AbstractClasses;
using FormsClone.CSharp.MainFunctionality.Questions.Models;

namespace FormsClone.CSharp.MainFunctionality.Questions.Services
{
    public class QuestionsService : BaseEntityService<Question>
    {
        protected override string Prefix => "question_";

        public QuestionsService(ILocalStorageService localStorage) : base(localStorage)
        {
        }

        public override async Task<List<Question>> SearchAsync(string searchTerm)
        {
            var allQuestions = await GetAllAsync();
            return allQuestions.Where(q =>
                q.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) 
                ).ToList();
        }
    }
}
