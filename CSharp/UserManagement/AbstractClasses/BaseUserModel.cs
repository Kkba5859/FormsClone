using FormsClone.CSharp.UserManagement.Interfaces; // Подключение интерфейса модели пользователя.
using System.ComponentModel.DataAnnotations; // Подключение пространства имен для аннотаций данных.

namespace FormsClone.CSharp.UserManagement.AbstractClasses
{
    public abstract class BaseUserModel : IUserModel // Абстрактный класс BaseUserModel реализует интерфейс IUserModel.
    {
        public int Id { get; set; } // Свойство идентификатора пользователя.

        [Required(ErrorMessage = "Username is required.")] // Аннотация, указывающая, что поле "Username" обязательно для заполнения.
        [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters.")] // Ограничение на длину строки для поля "Username".
        public string Username { get; set; } = string.Empty; // Свойство для имени пользователя, инициализируемое пустой строкой.

        [Required(ErrorMessage = "Password is required.")] // Аннотация, указывающая, что поле "Password" обязательно для заполнения.
        [StringLength(30, ErrorMessage = "Password cannot be longer than 30 characters.")] // Ограничение на длину строки для поля "Password".
        public string Password { get; set; } = string.Empty; // Свойство для пароля, инициализируемое пустой строкой.

        public bool IsAdmin { get; set; } // Свойство, указывающее, является ли пользователь администратором.

        protected BaseUserModel() { } // Защищенный конструктор по умолчанию.

        protected BaseUserModel(string username, string password, bool isAdmin) // Защищенный конструктор с параметрами для инициализации полей.
        {
            Username = username ?? throw new ArgumentNullException(nameof(username)); // Присваивание значения имени пользователя с проверкой на null.
            Password = password ?? throw new ArgumentNullException(nameof(password)); // Присваивание значения пароля с проверкой на null.
            IsAdmin = isAdmin; // Инициализация свойства IsAdmin.
        }
    }
}
