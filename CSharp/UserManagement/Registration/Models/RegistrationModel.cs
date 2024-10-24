    using FormsClone.CSharp.UserManagement.AbstractClasses;
    using System.ComponentModel.DataAnnotations;

    namespace FormsClone.CSharp.UserManagement.Registration.Models
    {
        public class RegistrationModel : BaseUserModel
        {
        [Required(ErrorMessage = "Электронная почта обязательна для заполнения.")]
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
        [StringLength(30, ErrorMessage = "Электронная почта не может превышать 30 символов.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтверждение пароля обязательно для заполнения.")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        public RegistrationResult RegistrationResult { get; set; } = new RegistrationResult(false);
            public ValidationResult ValidationResult { get; set; } = new ValidationResult(true);

            public RegistrationModel() : base() { }

            public RegistrationModel(string username, string email, string password, bool isAdmin)
                : base(username, password, isAdmin)
            {
                Email = email ?? throw new ArgumentNullException(nameof(email));
                ConfirmPassword = ConfirmPassword ?? throw new ArgumentNullException(nameof(ConfirmPassword));
            if (Password == ConfirmPassword)
                {
                    RegistrationResult = new RegistrationResult(true);
                    ValidationResult = new ValidationResult(true);
                }
            }
        }

        public class RegistrationResult
        {
            public bool Succeeded { get; }
            public IEnumerable<string> Errors { get; }

            public RegistrationResult(bool succeeded) : this(succeeded, new List<string>())
            {
            }

            public RegistrationResult(bool succeeded, IEnumerable<string>? errors)
            {
                Succeeded = succeeded;
                Errors = errors ?? new List<string>();
            }
        }

        public class ValidationResult
        {
            public bool IsValid { get; }
            public IEnumerable<string> Errors { get; }

            public ValidationResult(bool isValid, IEnumerable<string>? errors = null)
            {
                IsValid = isValid;
                Errors = errors ?? new List<string>();
            }
        }
    }
