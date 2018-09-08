using GenFu;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Shared.Contracts;
using System;
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
        public async Task GetAsync_ShouldReturnPaginatedListOfTodoItems()
        {
            // Arrange
            int pageNumber = 1, pageSize = 50;
            var todos = A.ListOf<TodoItem>(pageSize);
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.GetAsync(pageNumber, pageSize).Returns(todos);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Get(pageNumber, pageSize);
            var result = response.Result as ObjectResult;
            var value = result.Value as List<TodoItem>;
            var statusCode = result.StatusCode;

            // Assert
            Assert.IsNotNull(value);
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(value.Count, todos.Count);
            Assert.AreEqual(statusCode, 200);
        }

        [Test]
        public async Task GetAsync_ShouldReturnNotFoundWhenTodoItemIsMissing()
        {
            // Arrange
            TodoItem todo = null;
            var id = Guid.NewGuid().ToString();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.GetAsync(id).Returns(todo);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Get(id);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), response.Result);
        }

        [Test]
        public async Task GetAsync_ShouldReturnSingleTodoItemById()
        {
            // Arrange
            var todo = A.New<TodoItem>();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.GetAsync(todo.Id).Returns(todo);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Get(todo.Id);
            var result = response.Result as ObjectResult;
            var value = result.Value as TodoItem;
            var statusCode = result.StatusCode;

            // Assert
            Assert.IsNotNull(value);
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(value.Id, todo.Id);
            Assert.AreEqual(statusCode, 200);
        }

        [Test]
        public async Task PutAsync_ShouldReturnBadRequestWhenInputIsNull()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Put(id, null);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), response);
        }

        [Test]
        public async Task PutAsync_ShouldReturnNotFoundWhenTodoItemIsMissing()
        {
            // Arrange
            var todo = A.New<TodoItem>();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.Exists(i => i.Id == todo.Id).ReturnsForAnyArgs(false);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Put(todo.Id, todo);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), response);
        }

        [Test]
        public async Task PutAsync_ShouldReturnNoContentWhenTodoItemIsUpdated()
        {
            // Arrange
            var todo = A.New<TodoItem>();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.Exists(i => i.Id == todo.Id).ReturnsForAnyArgs(true);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Put(todo.Id, todo);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), response);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnNotFoundWhenTodoItemIsMissing()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.Exists(i => i.Id == id).ReturnsForAnyArgs(false);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Delete(id);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), response);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnNoContentWhenTodoItemIsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var todoItemsRepository = Substitute.For<IRepository<TodoItem>>();
            todoItemsRepository.Exists(i => i.Id == id).ReturnsForAnyArgs(true);
            var controller = new TodoItemsController(todoItemsRepository);

            // Act
            var response = await controller.Delete(id);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), response);
        }
    }
}
