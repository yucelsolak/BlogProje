using System.Collections.Generic;

namespace Core.Utilities.Results
{
    public class ValidationResult : IValidationResult
    {
        private readonly List<FieldError> _errors = new();
        public bool Success => _errors.Count == 0;
        public IReadOnlyList<FieldError> Errors => _errors;

        public void AddError(string field, string message) => _errors.Add(new FieldError(field, message));
    }
}
