using GenFu;
using NSubstitute;
using NUnit.Framework;
using Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi.Tests.Controllers
{
    [TestFixture]
    public class TodoItemsControllerTests
    {
        [Test]
        public async Task GetTest()
        {
            // Arrange
            int pageNumber = 1, pageSize = 50;
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.GetAsync(pageNumber, pageSize)
                               .Returns(Task.Run<IEnumerable<TodoItem>>(() => A.ListOf<TodoItem>(20)));
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var todos = await controller.Get();

            // Assert
            Assert.IsNotNull(todos);
        }
    }
}
