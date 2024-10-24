namespace FormsClone.CSharp.MainFunctionality.Interfaces
{
    public interface ITabService
    {
        string ActiveTab { get; set; }
        void SetActiveTab(string tab);
    }
}
