using SupaTrupa.WebAPI.Shared.Abstracts;
using SupaTrupa.WebAPI.Shared.Attributes;

namespace SupaTrupa.WebAPI.Tests.Shared.Entities
{
    [CollectionName("Person")]
    class Person : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Single { get; set; }
        public Address Address { get; set; }
    }
}
