﻿using GenFu;
using Microsoft.AspNetCore.Mvc;
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
            var value = result.Value as IEnumerable<TodoItem>;
            var statusCode = result.StatusCode;

            // Assert
            Assert.IsNotNull(value);
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(value, todos);
            Assert.AreEqual(statusCode, 200);
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
            Assert.AreEqual(value, todo);
            Assert.AreEqual(statusCode, 200);
        }
    }
}
