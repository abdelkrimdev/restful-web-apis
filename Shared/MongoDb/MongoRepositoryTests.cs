using System.Linq;
using System.Threading.Tasks;
using GenFu;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;

using SupaTrupa.WebAPI.Settings;
using SupaTrupa.WebAPI.Shared.Contracts;
using SupaTrupa.WebAPI.Shared.MongoDb;

using SupaTrupa.WebAPI.Tests.Shared.Entities;

#pragma warning disable RECS0030 // Suggests using the class declaring a static function when calling it

namespace SupaTrupa.WebAPI.Tests.Shared.MongoDb
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        IOptions<MongoDbSettings> _settings;
		IRepository<Person> _personRepository;

		[SetUp]
		public void Initialize()
		{
			_settings = Options.Create(new MongoDbSettings
			{
				User = "iodine",
				Pass = "secret",
				Host = "localhost",
				Port = "27017",
				Data = "supaTrupaLab"
			});

			_personRepository = new MongoRepository<Person>(_settings);

            A.Configure<Person>()
             .Fill(p => p.Single, true)
             .Fill(p => p.Address, A.New<Address>());
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
            var persons = A.ListOf<Person>();

			// Act
            await _personRepository.AddAsync(persons);

            // Assert
            Assert.AreEqual(persons.Count, _personRepository.Count());
        }

        [Test]
        public async Task UpdateAsyncTest()
		{
			// Arrange
            var persons = A.ListOf<Person>();
            await _personRepository.AddAsync(persons);

			foreach (var person in persons)
			{
				person.Single = false;
			}

			// Act
            await _personRepository.UpdateAsync(persons);

            // Assert
            var updatedPersons = await _personRepository.GetAsync(p => !p.Single);
			Assert.NotNull(updatedPersons);
            Assert.AreEqual(persons.Count, updatedPersons.Count());
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
			var persons = A.ListOf<Person>();
			await _personRepository.AddAsync(persons);

            // Act
            await _personRepository.DeleteAsync(p => p.Single);

			// Assert
            Assert.Zero(_personRepository.Count());
		}
	}
}
