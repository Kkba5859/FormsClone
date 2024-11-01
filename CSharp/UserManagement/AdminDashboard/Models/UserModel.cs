using FormsClone.CSharp.UserManagement.AbstractClasses; // Подключение абстрактного класса BaseUserModel.

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Models
{
    public class UserModel : BaseUserModel // Класс UserModel наследует от BaseUserModel.
    {
        public bool IsBlocked { get; set; } // Свойство, указывающее, заблокирован ли пользователь.

        public UserModel() : base() { } // Конструктор по умолчанию, вызывающий конструктор базового класса.

        public UserModel(string username, string password, bool isAdmin, bool isBlocked = false) // Конструктор с параметрами для инициализации полей.
            : base(username, password, isAdmin) // Вызов конструктора базового класса с параметрами.
        {
            IsBlocked = isBlocked; // Инициализация свойства IsBlocked.
        }
    }
}
