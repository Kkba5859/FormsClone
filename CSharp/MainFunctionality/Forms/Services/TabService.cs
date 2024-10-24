using FormsClone.CSharp.MainFunctionality.Interfaces;

namespace FormsClone.CSharp.MainFunctionality.Forms.Services
{
    public class TabService : ITabService
    {
        public string ActiveTab { get; set; } = "Questions";

        public void SetActiveTab(string tab)
        {
            ActiveTab = tab;
        }
    }
}


