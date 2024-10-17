namespace FormsClone.Models.Registration.Model
{
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