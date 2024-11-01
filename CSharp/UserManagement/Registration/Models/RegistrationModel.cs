using FormsClone.CSharp.UserManagement.AbstractClasses; // Подключение абстрактного класса BaseUserModel.
using System.ComponentModel.DataAnnotations; // Подключение для использования атрибутов валидации.

namespace FormsClone.CSharp.UserManagement.Registration.Models
{
    public class RegistrationModel : BaseUserModel // Класс RegistrationModel наследует от BaseUserModel.
    {
        [Required(ErrorMessage = "Электронная почта обязательна для заполнения.")] // Атрибут для обязательного поля.
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")] // Атрибут для проверки формата электронной почты.
        [StringLength(30, ErrorMessage = "Электронная почта не может превышать 30 символов.")] // Ограничение на длину электронной почты.
        public string Email { get; set; } = string.Empty; // Свойство для хранения электронной почты.

        [Required(ErrorMessage = "Подтверждение пароля обязательно для заполнения.")] // Атрибут для обязательного поля.
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")] // Сравнение пароля с подтверждением.
        public string ConfirmPassword { get; set; } = string.Empty; // Свойство для хранения подтверждения пароля.

        public RegistrationResult RegistrationResult { get; set; } = new RegistrationResult(false); // Результат регистрации.
        public ValidationResult ValidationResult { get; set; } = new ValidationResult(true); // Результат валидации.

        public RegistrationModel() : base() { } // Конструктор по умолчанию, вызывающий конструктор базового класса.

        public RegistrationModel(string username, string email, string password, bool isAdmin) // Конструктор с параметрами для инициализации полей.
            : base(username, password, isAdmin) // Вызов конструктора базового класса с параметрами.
        {
            Email = email ?? throw new ArgumentNullException(nameof(email)); // Инициализация электронной почты с проверкой на null.
            ConfirmPassword = ConfirmPassword ?? throw new ArgumentNullException(nameof(ConfirmPassword)); // Инициализация подтверждения пароля с проверкой на null.

            // Проверка совпадения пароля и его подтверждения.
            if (Password == ConfirmPassword)
            {
                RegistrationResult = new RegistrationResult(true); // Установка успешного результата регистрации.
                ValidationResult = new ValidationResult(true); // Установка успешного результата валидации.
            }
        }
    }

    public class RegistrationResult // Класс для хранения результата регистрации.
    {
        public bool Succeeded { get; } // Свойство, указывающее на успешность регистрации.
        public IEnumerable<string> Errors { get; } // Свойство для хранения списка ошибок.

        public RegistrationResult(bool succeeded) : this(succeeded, new List<string>()) // Конструктор для инициализации без ошибок.
        {
        }

        public RegistrationResult(bool succeeded, IEnumerable<string>? errors) // Конструктор для инициализации с ошибками.
        {
            Succeeded = succeeded; // Установка успешности.
            Errors = errors ?? new List<string>(); // Установка ошибок или пустого списка.
        }
    }

    public class ValidationResult // Класс для хранения результата валидации.
    {
        public bool IsValid { get; } // Свойство, указывающее на валидность.
        public IEnumerable<string> Errors { get; } // Свойство для хранения списка ошибок.

        public ValidationResult(bool isValid, IEnumerable<string>? errors = null) // Конструктор для инициализации результата валидации.
        {
            IsValid = isValid; // Установка валидности.
            Errors = errors ?? new List<string>(); // Установка ошибок или пустого списка.
        }
    }
}
