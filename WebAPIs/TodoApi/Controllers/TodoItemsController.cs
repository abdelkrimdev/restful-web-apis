using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        readonly IRepository<TodoItem> _todoItemsRepository;

        public TodoItemsController(IRepository<TodoItem> todoItemsRepository)
        {
            _todoItemsRepository = todoItemsRepository;
        }

        [HttpGet(Name = "GetTodos")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get(int pageNo = 1, int pageSize = 50)
        {
            var todos = await _todoItemsRepository.GetAsync(pageNo, pageSize);

            return Ok(todos);
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<ActionResult<TodoItem>> Get(string id)
        {
            var item = await _todoItemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost(Name = "CreateTodo)")]
        public async Task<IActionResult> Post([FromBody] TodoItem item)
        {
            if (item == null)
                return BadRequest();

            await _todoItemsRepository.AddAsync(item);

            return CreatedAtRoute("GetTodo", new { item.Id }, item);
        }

        [HttpPut("{id}", Name = "UpdateTodo")]
        public async Task<IActionResult> Put(string id, [FromBody] TodoItem item)
        {
            if (item == null)
                return BadRequest();

            if (!_todoItemsRepository.Exists(i => i.Id == id))
                return NotFound();

            if (item.Id == null || item.Id != id)
                item.Id = id;

            await _todoItemsRepository.UpdateAsync(item);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteTodo")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!_todoItemsRepository.Exists(i => i.Id == id))
                return NotFound();

            await _todoItemsRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
