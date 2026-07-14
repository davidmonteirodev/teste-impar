using System.Text.RegularExpressions;
using VehicleCRM.Domain.Common.ValueObjects;

namespace VehicleCRM.Domain.Customers.ValueObjects
{
    public sealed class Phone : ValueObject
    {
        public string Value { get; }

        private Phone(string value)
        {
            Value = value;
        }

        public static Phone Create(string? phone)
        {
            var phoneOnlyNumbers = NormalizePhone(phone);
            return new Phone(phoneOnlyNumbers);
        }

        private static string NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            return Regex.Replace(phone, @"[^\d]", string.Empty);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Phone phone) => phone.Value;
    }
}
