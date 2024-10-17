namespace FormsClone.Models.Registration
{
    public class RegistrationResult
    {
        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }

        // Constructor for success result
        public RegistrationResult(bool succeeded) : this(succeeded, new List<string>())
        {
        }

        // Constructor for result with errors
        public RegistrationResult(bool succeeded, IEnumerable<string>? errors)
        {
            Succeeded = succeeded;
            Errors = errors ?? new List<string>();
        }
    }
}