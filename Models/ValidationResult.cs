using System.Collections.Generic;
using System.Linq;

namespace Traveler.Models
{
    /// <summary>
    /// Represents the outcome of validating a <see cref="TravelRequest"/>.
    /// Carries all accumulated error messages so the user sees every problem at once.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; }

        private ValidationResult(List<string> errors)
        {
            Errors = errors;
        }

        /// <summary>Returns a successful <see cref="ValidationResult"/> with no errors.</summary>
        public static ValidationResult Ok() => new ValidationResult(new List<string>());

        /// <summary>Returns a failed <see cref="ValidationResult"/> containing one or more error messages.</summary>
        public static ValidationResult Fail(IEnumerable<string> errors) =>
            new ValidationResult(errors.ToList());

        public override string ToString() =>
            IsValid ? "Valid" : string.Join("; ", Errors);
    }
}
