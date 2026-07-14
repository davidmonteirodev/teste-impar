using System.Text.RegularExpressions;
using VehicleCRM.Domain.Common.ValueObjects;

namespace VehicleCRM.Domain.Customers.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string? email)
        {
            var normalizedEmail = NormalizeEmail(email);
            return new Email(normalizedEmail);
        }

        private static string NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            return email.Trim().ToLowerInvariant();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
