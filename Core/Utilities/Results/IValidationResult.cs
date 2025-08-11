using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IValidationResult
    {
        bool Success { get; }
        IReadOnlyList<FieldError> Errors { get; }
    }

    public record FieldError(string Field, string Message);
}
