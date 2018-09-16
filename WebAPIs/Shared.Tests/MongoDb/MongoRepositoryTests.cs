using GenFu;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;
using Shared.Contracts;
using Shared.MongoDb;
using Shared.Tests.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Tests.MongoDb
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        IOptions<MongoSettings> _settings;
        IRepository<Person> _personRepository;

        [SetUp]
        public void Initialize()
        {
            _settings = Options.Create(new MongoSettings
            {
                Host = "localhost",
                Port = "27017",
                User = "TestUser",
                Pass = "Secret",
                Data = "TestDatabase"
            });

            _personRepository = new MongoRepository<Person>(_settings);

            A.Configure<Person>()
             .Fill(p => p.Address, A.New<Address>())
             .Fill(p => p.Single, true);
        }

        [TearDown]
        public void DropDataBase()
        {
            var mongoUrl = MongoUrl.Create(MongoService.GetConnectionString(_settings));
            MongoService.GetClientFromUrl(mongoUrl).DropDatabase(mongoUrl.DatabaseName);
        }

        [Test]
        public async Task GetAsyncTest()
        {
            // Arrange
            var person = A.New<Person>();
            await _personRepository.AddAsync(person);

            // Act
            var addedPerson = await _personRepository.GetAsync(person.Id);

            // Assert
            Assert.NotNull(addedPerson);
            Assert.AreEqual(person.Id, addedPerson.Id);
            Assert.AreEqual(person.Address.Id, addedPerson.Address.Id);
        }

        [Test]
        public async Task GetAsyncByPageTest()
        {
            // Arrange
            int pageNumber = 2, pageSize = 15;
            var people = A.ListOf<Person>(100);
            await _personRepository.AddAsync(people);

            // Act
            var peoplePage = await _personRepository.GetAsync(pageNumber, pageSize);

            // Assert
            Assert.NotNull(peoplePage);
            Assert.AreEqual(peoplePage.Count(), pageSize);
            Assert.AreEqual(people.ElementAt((pageNumber - 1) * pageSize).Id, peoplePage.ElementAt(0).Id);
        }

        [Test]
        public async Task AddEntityAsyncTest()
        {
            // Arrange
            var person = A.New<Person>();

            // Act
            await _personRepository.AddAsync(person);

            // Assert
            Assert.True(_personRepository.Exists(p => p.Id == person.Id));
        }

        [Test]
        public async Task AddEntitiesAsyncTest()
        {
            // Arrange
            var people = A.ListOf<Person>();

            // Act
            await _personRepository.AddAsync(people);

            // Assert
            Assert.AreEqual(people.Count, _personRepository.Count());
        }

        [Test]
        public async Task UpdateAsyncTest()
        {
            // Arrange
            var people = A.ListOf<Person>();
            await _personRepository.AddAsync(people);

            foreach (var person in people)
            {
                person.Single = false;
                person.Address.Country = person.Address.Country.ToUpper();
            }

            // Act
            await _personRepository.UpdateAsync(people);

            // Assert
            var updatedPeople = await _personRepository.GetAsync(p => !p.Single);

            Assert.NotNull(updatedPeople);
            Assert.AreEqual(people.Count, updatedPeople.Count());

            for (int index = 0, count = people.Count; index < count; index++)
            {
                Assert.AreEqual(people[index].Address.Country,
                                updatedPeople.ElementAt(index).Address.Country);
            }
        }

        [Test]
        public async Task DeleteEntityAsyncTest()
        {
            // Arrange
            var person = A.New<Person>();
            await _personRepository.AddAsync(person);

            // Act
            await _personRepository.DeleteAsync(person);

            // Assert
            Assert.False(_personRepository.Exists(p => p.Id == person.Id));
        }

        [Test]
        public async Task DeleteEntitiesAsyncTest()
        {
            // Arrange
            var people = A.ListOf<Person>();
            await _personRepository.AddAsync(people);

            // Act
            await _personRepository.DeleteAsync(p => p.Single);

            // Assert
            Assert.Zero(_personRepository.Count());
        }
    }
}
