using Shared.Abstracts;

namespace Shared.Tests.Entities
{
    class Person : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Single { get; set; }
    }
}
