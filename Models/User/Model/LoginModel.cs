namespace FormsClone.Models.User.Model
{
    public class LoginModel
    {
        // Пустой конструктор (не требует аргументов)
        public LoginModel() { }

        // Конструктор с параметрами
        public LoginModel(string? username, string? password, bool isAdmin)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            IsAdmin = isAdmin;
        }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }  // Admin flag for login
    }
}