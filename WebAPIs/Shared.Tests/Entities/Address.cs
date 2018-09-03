using Shared.Abstracts;

namespace Shared.Tests.Entities
{
    public class Address : Entity
    {
        public string AddressLine { get; internal set; }
        public string PostCode { get; internal set; }
        public string City { get; internal set; }
        public string Country { get; internal set; }
    }
}
